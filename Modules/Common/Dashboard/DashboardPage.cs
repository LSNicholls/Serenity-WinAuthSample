
using Dapper;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NUglify.JavaScript.Syntax;
using Serenity.Extensions.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using static Dapper.SqlMapper;
using MyRow = WinAuthSample.Administration.UserRow;

namespace WinAuthSample.Common.Pages;

[Route("Dashboard/[action]")]
public class DashboardPage : Controller
{
    [PageAuthorize, HttpGet, Route("~/")]
    public ActionResult Index([FromServices] ITwoLevelCache cache,
                              [FromServices] ISqlConnections sqlConnections,
                              [FromServices] IUserRetrieveService userRetriever,
                              [FromServices] IUserAccessor userAccessor)
    {

        ArgumentNullException.ThrowIfNull(cache);

        ArgumentNullException.ThrowIfNull(sqlConnections);

        var o = MyRow.Fields;
        var u = userRetriever.ByUsername(User.Identity.Name);
        //  bool onDomain = (bool)config.GetValue(typeof(bool), "WinAuthSettings:DomainConnected");
        // we'll check to see if we need to refresh user information now.

        //  the non-user-specific info for the dashboard:

        // theoretically you have real work going on here, which is not user-specific.
        // I'm just adding a  hokey example to illustrate that because I don't have 
        // Northwind db in this sample app:
        var cachedModel = cache.GetLocalStoreOnly("DashboardPageModel", TimeSpan.FromMinutes(300),
           o.GenerationKey, () =>
           {
               var model = new DashboardPageModel();
               using (var connection = sqlConnections.NewFor<MyRow>())
               {
                   model.OpenOrders = (int)connection.ExecuteScalar("select min(UserId) as MinId from Users");
                   var closedOrders = (int)connection.ExecuteScalar("select max(UserId) as MaxId from Users");
                   var totalOrders = model.OpenOrders + closedOrders;
                   model.ClosedOrderPercent = (double)Math.Round(totalOrders == 0 ? 100 :
                       ((double)closedOrders / totalOrders * 100), 2);
                   model.CustomerCount = 58;
                   model.ProductCount = 22;
               }
               model.IsDomainConnected = AppServices.AuthUtils.IsDomainConnected();
               return model;
           });


        using var connection = sqlConnections.NewFor<UserPreferenceRow>();
        var cmd = connection.CreateCommand();

        // here is where we leverage the "DashboardNeedsRefresh" flag that we may have set somewhere else
        // indicating that the Dashboard caching mechanism doesn't know about some pertinent changes
        // for this application, whatever they might be, that would affect the model shown to this
        // particular user.
        //  (I'm sure there is a way to do this using UserPreferenceStorage, without direct SQL, since
        //  the repository code will delete if the value is null or empty, but why bother?  It might be worth
        //  doing it that way if we were actually looking at the value instead of just treating the row as a flag.)

        bool r = (bool)connection.ExecuteScalar("declare @u as int = " + u.Id + ", @n as varchar(200) = 'DashboardNeedsRefresh'; " +
                                         " select cast(1 as bit) as Value from UserPreferences " +
                                         " where UserId = @u  and Name = @n " +
                                         " union all select cast(0 as bit)" +
                                          "order by 1 desc;");

        if (r == true)
        {
            // This is just illustrative.
            // here, do **whatever needs to be done** regarding special refresh of user-related Dashboard items for the particular application.

            // Then remove the preference that you set somewhere else in the application :

            connection.ExecuteScalar(
                "declare @u as int = " + u.Id + ", @n as varchar(200) = 'DashboardNeedsRefresh'; " +
                " delete from UserPreferences where  " +
                " UserId = @u and Name = @n;  ");
        }

        // you could add a second cache item to the local store here, or re-get every time, which is what I am doing in the example, considering
        // it's pretty lightweight and I already called the userRetrieveService to get the user definition.  It all depends on what you want to 
        // show, but I do recommend you cache on a separate schedule for the user as compared with whatever application stuff you're doing above:
    
     
        var parts = u.Username.Split('\\');
        bool appUser = (parts.Length == 1);
        var identity = new HttpContextAccessor().HttpContext.User.Identity;

        var uAttr = new DashboardUserModel
        {
            UserIdentifier = u.Id,
            UserName = u.Username,
            DisplayName = u.DisplayName,
            IsActive = (u.IsActive == 1),
            IsApplicationUser =  appUser,
            UserDebugInfo = "<h6/>" + (appUser ? "<h6>Application User: " + identity.Name : "Windows Identity Current: " + WindowsIdentity.GetCurrent().Name +
                              "<br/>HttpContext User.Identity: " + identity.Name +
                               "<br/>User Accessor.User.Identity: " + userAccessor.User.Identity.Name +
                               "<br/>new WindowsIdentity used in code if on domain: " + ((u.Username.Contains('\\')) ?
                               parts[1] + '@' + parts[0] : "(none)") ) +
                               "<br/>On Domain: " + cachedModel.IsDomainConnected.ToString() + 
                              "</h6>"
        };
          
        if (! uAttr.IsApplicationUser) { 
        StringBuilder groups = new("<h6>Windows Network Group Info for userAccessor:</h6>");

            // WindowsIdentity currIdentity =  (WindowsIdentity) userAccessor.User.Identity;
            
            // WindowsIdentity     currIdentity = new WindowsIdentity("linich@Eden") ;
            WindowsIdentity currIdentity = (cachedModel.IsDomainConnected ?  new WindowsIdentity(parts[1] + '@' + parts[0]) : (WindowsIdentity)userAccessor.User.Identity);

            foreach (var groupId in currIdentity.Groups)
        {
            groups.Append(groupId.ToString() + " | ");
            if (groupId.IsValidTargetType(typeof(System.Security.Principal.NTAccount)))
            {

                try
                {
                    groups.Append("groupname : " + groupId.Translate(typeof(System.Security.Principal.NTAccount)) );
                }
                catch (Exception ex)
                {
                    groups.Append("error : " + ex.Message);

                }
            }

            else
            {
                groups.Append(" : not valid System.Security.Principal.NTAccount");
            }
            groups.Append("<br/>");
        }
            groups.Append("<hr/>");
        foreach (ClaimsIdentity i in HttpContext.User.Identities)
            {
                groups.Append(i.Name  + "<br/>");
            }
            uAttr.UserDebugInfo += groups.ToString();
        }



        cachedModel.User = uAttr;
        

        return View(MVC.Views.Common.Dashboard.DashboardIndex, cachedModel);
    }
}
