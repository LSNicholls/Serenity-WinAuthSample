using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Security.Principal;
using WinAuthSample.Administration;

//winauth:
using MyRow = WinAuthSample.Administration.UserRow;

namespace WinAuthSample.AppServices;

public class UserAccessor(IHttpContextAccessor httpContextAccessor, [FromServices] ISqlConnections sqlconnections) : IUserAccessor, IImpersonator
{
    private readonly ImpersonatingUserAccessor impersonator = new(new HttpContextUserAccessor(httpContextAccessor),
            new HttpContextItemsAccessor(httpContextAccessor));


    // winauth:
    private readonly ISqlConnections sqlConnections = sqlconnections;
    private static MyRow.RowFields Fld { get { return MyRow.Fields; } }

    // winauth changes:
    public ClaimsPrincipal User => (GetUser(impersonator.User, sqlConnections));

    private static ClaimsPrincipal GetUser(ClaimsPrincipal user, ISqlConnections sqlConnections)
    {
        if ((!user.HasClaim(x => x.Type == ClaimTypes.NameIdentifier))
            && (user.Identity.Name.Contains('\\')))
        {
            WindowsIdentity ident = (WindowsIdentity) user.Identity;

            var u = sqlConnections.NewByKey("Default").TrySingle<MyRow>(
                 new Criteria(Fld.Username) == ident.Name);
            if (u == null && (UserRetrieveService.RefreshCurrentUserRolesByNetworkGroup(ident.Name, sqlConnections.NewFor<UserRow>(), false,ident)))
            {
                u = sqlConnections.NewByKey("Default").TrySingle<MyRow>(
                 new Criteria(Fld.Username) == ident.Name);
            }

            string id = u.UserId.ToString();
            ident.AddClaim(new Claim(ClaimTypes.NameIdentifier, id));  
           
        }
        return user;
    }
    // end of change
    public void Impersonate(ClaimsPrincipal user)
    {
        impersonator.Impersonate(user);
    }

    public void UndoImpersonate()
    {
        impersonator.UndoImpersonate();
    }
}