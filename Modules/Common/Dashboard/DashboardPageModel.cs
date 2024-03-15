
namespace WinAuthSample.Common;

public class DashboardPageModel
{
    public int OpenOrders { get; set; }
    public double ClosedOrderPercent { get; set; } // winauth cosmetic change
    public int CustomerCount { get; set; }
    public int ProductCount { get; set; }

    public bool IsDomainConnected { get; set; }
    public DashboardUserModel User { get; set; } // winauth sample addition
}

//winauth sample addition
public class DashboardUserModel
{
    public string UserIdentifier { get; set; }

    public string UserName { get; set; }
    public string DisplayName { get; set; }
    public bool IsActive { get; set; }

    public bool IsApplicationUser { get; set; }
    public string UserDebugInfo { get; set; }
    // public bool IsDomainUser { get; set; }  //etc etc


}