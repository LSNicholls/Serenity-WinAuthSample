
using Microsoft.Extensions.Configuration;
using System.Data.Common;
using System.Security.Principal;
using MyRow = WinAuthSample.Administration.UserRow;


namespace WinAuthSample.AppServices 
{
    [Route("/User/Auth/[action]")]
    [ConnectionKey(typeof(MyRow)), ServiceAuthorize(typeof(MyRow))]
    public class AuthUtilsEndpoint: ServiceEndpoint
    {
        

        [HttpPost]
        [ServiceAuthorize("?")] // see note for an action below about this attribute.
        public ActionResult RemoveUserCache(int? UserId, string UserName, Boolean RefreshRoles, [FromServices] ISqlConnections sqlConnections)
        {

            var retval = "ok";
            try
            {
                var conn = sqlConnections.NewFor<MyRow>();
                UserRetrieveService.RemoveCachedUser(Cache, UserId, UserName);

                
                if (UserName.Contains('\\') )
                {
                    
                    if (RefreshRoles && UserId.HasValue)
                    {
                        UserRetrieveService.RefreshCurrentUserRolesByNetworkGroup(UserName, conn, false, (WindowsIdentity) Context.User.Identity);
                    }

                }

                RequestDashboardRefresh(UserId, conn);

            }
            catch (Exception ex)
            {
                retval = "failed: " + ex.Message;
            }
            return new JsonResult(new { result = retval });

        }

        [HttpPost]
        [ServiceAuthorize("?")] // see note in the section action below about this attribute.
        public ActionResult AdminRefreshUser(int? UserId, string UserName, bool RemoveOtherRoles, [FromServices] ISqlConnections sqlConnections, [FromServices] IUserAccessor userAccessor) //,  [FromServices] IConfiguration config
        {

            var retval = "ok";


            try
            {
                // the admin version always removes the user cache AND roles; the only question is whether it will remove roles
                // that are not explicitly set by Windows Group.
                var conn = sqlConnections.NewFor<MyRow>();

                UserRetrieveService.AdminRefreshUser(Cache, userAccessor, UserId, UserName,  conn,  RemoveOtherRoles); //config,

                RequestDashboardRefresh(UserId, conn);


            }
            catch (Exception ex)
            {
                retval = "failed: " + ex.Message;
            }
            return new JsonResult(new { result = retval });

        }


        [HttpPost, HttpGet]
        [ServiceAuthorize("?")]
        //This method isn't actually called anywhere in the sample, but will likely be necessary for you.
        //You might want to put it in a different endpoint.
        
        // What you want to do here is invalidate any lists that depend on permissions
        // after a potential change to user rights & privileges.
        // The contents here would be specific to each application.
        // It gets the ServiceAuthorize("?") attribute and HttpGet if you want some other/external 
        // admin web application to be able to call this method, typically using an app pool identity.
        public ActionResult InvalidateAllUserListsCache()
        {

            var retval = "ok";
            try
            {
               
                // In these example calls, you have a couple of filtered lists used for lookup purposes:
                /*
                Cache.ExpireGroupItems(YourApp.Setup.LookupUserListRow.Fields.GenerationKey);
                Cache.ExpireGroupItems(YourApp.Setup.ActiveUserListRow.Fields.GenerationKey);
                */
            }
            catch (Exception ex)
            {
                retval = "failed: " + ex.Message;
            }
            return new JsonResult(new { result = retval });
        }

        private static void RequestDashboardRefresh(int? UserId, IDbConnection conn)
        {

            if (UserId.HasValue)
            {
                conn.Execute("declare @u as int = " + UserId.ToString() + ", @n as varchar(200) = 'DashboardNeedsRefresh'; " +
                                    " if not exists (select 1 as Value from UserPreferences " +
                                    " where UserId = @u  and Name = @n)  " +
                                    " insert into UserPreferences (UserId, PreferenceType, Name, Value) " +
                                    " select @u, 'UserPreferenceStorage', @n, 'YES' ;");
            }

        }
    }

}