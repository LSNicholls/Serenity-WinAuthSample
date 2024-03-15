using Microsoft.Extensions.Logging;
using WinAuthSample.Administration;

namespace WinAuthSample.AppServices;

public class UserPasswordValidator : IUserPasswordValidator
{
    public UserPasswordValidator(ITwoLevelCache cache, ISqlConnections sqlConnections, IUserRetrieveService userRetriever, 
        ILogger<UserPasswordValidator> log = null, IDirectoryService directoryService = null)
    {
        Cache = cache ?? throw new ArgumentNullException(nameof(cache));
        SqlConnections = sqlConnections ?? throw new ArgumentNullException(nameof(sqlConnections));
        UserRetriever = userRetriever ?? throw new ArgumentNullException(nameof(userRetriever));
        DirectoryService = directoryService;
        Log = log;
    }

    protected ITwoLevelCache Cache { get; }
    public ISqlConnections SqlConnections { get; }
    protected IUserRetrieveService UserRetriever { get; }
    protected IDirectoryService DirectoryService { get; }
    protected ILogger<UserPasswordValidator> Log { get; }

    public PasswordValidationResult Validate(ref string username, string password)
    {
        if (username.IsTrimmedEmpty())
            return PasswordValidationResult.EmptyUsername;
        //winauth changes:
        UserDefinition user = (UserDefinition)UserRetriever.ByUsername(username);
        if (!(user == null))
        {
            if (user.HasPassword || user.IsDomainUser)
                return ValidateExistingUser(ref username, password, user);
            if (string.IsNullOrEmpty(password))
                return PasswordValidationResult.EmptyPassword;

        }

        return ValidateFirstTimeUser(ref username, password);
    }

    private PasswordValidationResult ValidateExistingUser(ref string username, string password, UserDefinition user)
    {
        username = user.Username;

        if (user.IsActive != 1)
        {
            Log?.LogError("Inactive user login attempt: {username}", username);
            return PasswordValidationResult.InactiveUser;
        }

        // winauth addition:
        if (user.IsDomainUser && UserRetrieveService.RefreshCurrentUserRolesByNetworkGroup(username, SqlConnections.NewFor<UserRow>()))
        {
            return PasswordValidationResult.Valid;
        }
        // end winauth

        // prevent more than 50 invalid login attempts in 30 minutes
        var throttler = new Throttler(Cache.Memory, "ValidateUser:" + username.ToLowerInvariant(), TimeSpan.FromMinutes(30), 50);
        if (!throttler.Check())
            return PasswordValidationResult.Throttle;

        bool validatePassword() => UserHelper.CalculateHash(password, user.PasswordSalt)
            .Equals(user.PasswordHash, StringComparison.OrdinalIgnoreCase);

        if (user.Source == "site" || user.Source == "sign" || DirectoryService == null)
        {
            if (validatePassword())
            {
                throttler.Reset();
                return PasswordValidationResult.Valid;
            }

            return PasswordValidationResult.Invalid;
        }

        if (user.Source != "ldap")
            return PasswordValidationResult.UnknownSource;

        if (!string.IsNullOrEmpty(user.PasswordHash) &&
            user.LastDirectoryUpdate != null &&
            user.LastDirectoryUpdate.Value.AddHours(1) >= DateTime.Now)
        {
            if (validatePassword())
            {
                throttler.Reset();
                return PasswordValidationResult.Valid;
            }

            return PasswordValidationResult.Invalid;
        }

        DirectoryEntry entry;
        try
        {
            entry = DirectoryService.Validate(username, password);
            if (entry == null)
                return PasswordValidationResult.Invalid;

            throttler.Reset();
        }
        catch (Exception ex)
        {
            Log?.LogError(ex, "Error on directory access");

            // couldn't access directory. allow user to login with cached password
            if (!user.PasswordHash.IsTrimmedEmpty())
            {
                if (validatePassword())
                {
                    throttler.Reset();
                    return PasswordValidationResult.Valid;
                }

                return PasswordValidationResult.Invalid;
            }

            throw;
        }

        try
        {
            string salt = user.PasswordSalt.TrimToNull();
            var hash = UserHelper.GenerateHash(password, ref salt);
            var displayName = entry.FirstName + " " + entry.LastName;
            var email = entry.Email.TrimToNull() ?? user.Email ?? (username + "@yourdefaultdomain.com");

            using var connection = SqlConnections.NewFor<UserRow>();
            using var uow = new UnitOfWork(connection);
            var fld = UserRow.Fields;
            new SqlUpdate(fld.TableName)
                .Set(fld.DisplayName, displayName)
                .Set(fld.PasswordHash, hash)
                .Set(fld.PasswordSalt, salt)
                .Set(fld.Email, email)
                .Set(fld.LastDirectoryUpdate, DateTime.Now)
                .WhereEqual(fld.UserId, user.UserId)
                .Execute(connection, ExpectedRows.One);

            uow.Commit();

            UserRetrieveService.RemoveCachedUser(Cache, user.UserId, username);

            return PasswordValidationResult.Valid;
        }
        catch (Exception ex)
        {
            Log?.LogError(ex, "Error while updating directory user");
            return PasswordValidationResult.Valid;
        }
    }

    private PasswordValidationResult ValidateFirstTimeUser(ref string username, string password)
    {
        //winauth addition which probably doesn't ever get  hit by Windows users and will return false quickly for app identities.
        if (UserRetrieveService.RefreshCurrentUserRolesByNetworkGroup(username, SqlConnections.NewFor<UserRow>()))
        {
            return PasswordValidationResult.Valid;
        }
        //

        var throttler = new Throttler(Cache.Memory, "ValidateUser:" + username.ToLowerInvariant(), TimeSpan.FromMinutes(30), 50);
        if (!throttler.Check())
            return PasswordValidationResult.Throttle;

        if (DirectoryService == null)
            return PasswordValidationResult.Invalid;

        DirectoryEntry entry;
        try
        {
            entry = DirectoryService.Validate(username, password);
            if (entry == null)
                return PasswordValidationResult.Invalid;

            throttler.Reset();
        }
        catch (Exception ex)
        {
            Log?.LogError(ex, "Error on directory first time authentication");
            return PasswordValidationResult.DirectoryError;
        }

        try
        {
            string salt = null;
            var hash = UserHelper.GenerateHash(password, ref salt);
            var displayName = entry.FirstName + " " + entry.LastName;
            var email = entry.Email.TrimToNull() ?? (username + "@yourdefaultdomain.com");
            username = entry.Username.TrimToNull() ?? username;

            using var connection = SqlConnections.NewFor<UserRow>();
            using var uow = new UnitOfWork(connection);
            var userId = (int)connection.InsertAndGetID(new UserRow
            {
                Username = username,
                Source = "ldap",
                DisplayName = displayName,
                Email = email,
                PasswordHash = hash,
                PasswordSalt = salt,
                IsActive = 1,
                InsertDate = DateTime.Now,
                InsertUserId = 1,
                LastDirectoryUpdate = DateTime.Now
            });

            uow.Commit();

            UserRetrieveService.RemoveCachedUser(Cache, userId, username);

            return PasswordValidationResult.Valid;
        }
        catch (Exception ex)
        {
            Log?.LogError(ex, "Error while importing directory user");
            return PasswordValidationResult.DirectoryError;
        }
    }
}