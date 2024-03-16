using System.Globalization;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using MyRow = WinAuthSample.Administration.UserRow;
using Microsoft.Win32.SafeHandles;
using NUglify.Helpers;

using Serenity.Abstractions;
using NUglify.JavaScript.Syntax;



namespace WinAuthSample.AppServices;

public class UserRetrieveService(ITwoLevelCache cache, ISqlConnections sqlConnections) : IUserRetrieveService
{
    private static MyRow.RowFields Fld { get { return MyRow.Fields; } }

    protected ITwoLevelCache Cache { get; } = cache;
    protected ISqlConnections SqlConnections { get; } = sqlConnections;
    protected static bool DomainConnected { get; } = AppServices.AuthUtils.IsDomainConnected(); 
      

    // winauth method changes
    private static UserDefinition GetFirst(IDbConnection connection, BaseCriteria criteria)
    {
        var user = connection.TrySingle<MyRow>(criteria);

        // new block of code:
        if (user == null)
        {

            HttpContextAccessor httpContextAccessor = new();
            WindowsIdentity ident = (WindowsIdentity)httpContextAccessor.HttpContext.User.Identity;

            /*
             * the procedure will create a new row on the fly
             * for the current windows identity, initially with no permissions,
             * and add groups, which will implicitly add some permissions if
             * the groups are registered
             */

            RefreshCurrentUserRolesByNetworkGroup(ident.Name, connection, false, ident);

            // now re-get the user

            connection.EnsureOpen();

            user = connection.TrySingle<MyRow>(criteria);
        }
        // end of new block of code

        if (user != null)
            return new UserDefinition
            {
                UserId = user.UserId.Value,
                Username = user.Username,
                Email = user.Email,
                UserImage = user.UserImage,
                DisplayName = user.DisplayName,
                IsActive = user.IsActive.Value,
                Source = user.Source,
                PasswordHash = user.PasswordHash,
                PasswordSalt = user.PasswordSalt,
                UpdateDate = user.UpdateDate,
                LastDirectoryUpdate = user.LastDirectoryUpdate
                //win auth add here whatever you added into UserDefinition
            };

        return null;
    }

    public IUserDefinition ById(string id)
    {
        return Cache.Get("UserByID_" + id, TimeSpan.Zero, TimeSpan.FromDays(1), Fld.GenerationKey, () =>
        {
            using var connection = SqlConnections.NewByKey("Default");
            return GetFirst(connection, new Criteria(Fld.UserId) == int.Parse(id, CultureInfo.InvariantCulture));
        });
    }

    public IUserDefinition ByUsername(string username)
    {
        if (username.IsEmptyOrNull())
            return null;

        return Cache.Get("UserByName_" + username.ToLowerInvariant(),
            TimeSpan.Zero, TimeSpan.FromDays(1), Fld.GenerationKey, () =>
        {
            using var connection = SqlConnections.NewByKey("Default");
            var thisUser = GetFirst(connection, new Criteria(Fld.Username) == username);
            // winauth change: re-cache the user by ID based on this information
            var thisUserId = Cache.Get("UserByID_" + thisUser.UserId.ToStringInvariant(), TimeSpan.Zero, TimeSpan.FromDays(1), Fld.GenerationKey, () =>
            {
                return GetFirst(connection, new Criteria(Fld.UserId) == thisUser.UserId);
            });
            return thisUser;
        });
    }

    //winauth changes:
    public static void RemoveCachedUser(ITwoLevelCache cache, int? userId, string username)
    {
        if (userId != null)
        {
            cache.Remove("UserByID_" + userId);
            cache.Remove("LeftNavigationModel:NavigationItems:" + userId);
            cache.Remove("UserPermissions:" + userId);
            cache.Remove("UserRoles:" + userId);
        }

        if (username != null)
            cache.Remove("UserByName_" + username.ToLowerInvariant());
    }

    // winauth new methods start here:

    public static bool AdminRefreshUser(ITwoLevelCache cache, IUserAccessor adminUser, int? userId, string username, 
         IDbConnection connection, bool RemoveOtherRoles = false) // IConfiguration config,
    {
        bool retval = true;
        RemoveCachedUser(cache, userId, username);

        if (username.Contains('\\') && (!username.Equals(adminUser.User.Identity.Name, StringComparison.InvariantCultureIgnoreCase)))
        {
           
            var parts = username.Split('\\');
            //bool onDomain = (bool)config.GetValue(typeof(bool), "WinAuthSettings:DomainConnected");
            
            var impersonator = (IImpersonator)adminUser;
            var identw = ( DomainConnected ? new WindowsIdentity(parts[1] + '@' + parts[0]) :
                  WindowsIdentity.GetCurrent());
            identw.AddClaim(new Claim("NameIdentifier", userId.ToString()));
            impersonator.Impersonate(new ClaimsPrincipal(identw));
            retval = RefreshCurrentUserRolesByNetworkGroup(username, connection, RemoveOtherRoles , identw);
            impersonator.UndoImpersonate();

        }


        return retval;
    }
    public static bool RefreshCurrentUserRolesByNetworkGroup(string username, IDbConnection connection, bool RemoveOtherRoles = false, WindowsIdentity ident = null)
    {
        if (username.Contains('\\'))
        {
            /*
             * here is where we gather the network groups
             * and call the proc to refresh user roles based on this set of network groups.
             * We have to be careful not to do this too often.  How to decide when?
             * And do we have to remove cached user?
             * For now we are going to continue to do this in the nav model area,
             * since this is where it causes a problem.
             */
            //var x = WindowsIdentity.GetCurrent().Claims; // this, or whatever identity, would give us SIDs which we don't want.
            //var identity = WindowsIdentity.GetCurrent(); // can't use WindowsIdentity.GetCurrent(), it's giving us the App Pool identity.
            WindowsIdentity identity;
            if (ident == null)
            {
                HttpContextAccessor httpContextAccessor = new();
                identity = (WindowsIdentity)httpContextAccessor.HttpContext.User.Identity;
            }
            else
            {
                identity = ident;
            }
            StringBuilder groups = new();


            foreach (var groupId in identity.Groups)
            {
                if (groupId.IsValidTargetType(typeof(System.Security.Principal.NTAccount)))
                {
                    try
                    {
                        groups.Append(groupId.Translate(typeof(System.Security.Principal.NTAccount)) + "|");
                    }
                    catch
                    {
                        // don't worry about this;  we are just trying to solve the
                        // problem that not all SIDs can be validated:
                        // IdentityNotMappedException: Some or all identity references could not be translated.
                        // We'll just swallow these for now.

                    }
                }
            }

            if (groups.Length > 0)
            {
                String initials = GetUserInitials(username);


                connection.Execute("dbo.p_SetupWindowsNetworkUser", param: new
                {
                    UserName = username,
                    UserInitials = initials,
                    GroupList = groups.ToString(),
                    GroupListDelimiter = "|",
                    RemoveNonPermittedRolesPerGroups = RemoveOtherRoles

                }, commandType: System.Data.CommandType.StoredProcedure);


            }
            return true;
        }
        else
        {
            return false;
        }
    }


    // this is something  you may not need, it's just a worked example.
    private static string GetUserInitials(string tUser)
    {
        // can ONLY BE TWO CHARS MAX - four in the database, but the procedure
        // will need to add a tiebreaker if necessary.
        string uname = tUser[(tUser.LastIndexOf('\\') + 1)..];

        string firstChar = uname[..1];
        string secondChar;
        if (uname.Contains(' '))
        {
            secondChar = uname.Split(' ')[1][..1];
        }
        else
        {
            secondChar = "";
        }

        return (firstChar + secondChar).ToUpper();

    }
}
