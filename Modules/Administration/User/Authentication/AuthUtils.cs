namespace WinAuthSample.AppServices;

public class AuthUtils
{
    // this may not be the best way to do this, but I couldn't find another way, short of configuring in appsettings and reading it out
    // (you'll find that method in earlier commits).
    public static bool IsDomainConnected() {

        bool success = true;
        try
        {
            var ctx = System.DirectoryServices.ActiveDirectory.Domain.GetComputerDomain();
        }
        catch (Exception)
        {
            success = false;
        }
              
        return success;

        } 
}
