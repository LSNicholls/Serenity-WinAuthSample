using Newtonsoft.Json;

namespace WinAuthSample.Administration;

public class RolePermissionUpdateRequest : ServiceRequest
{
    public int? RoleID { get; set; }
    public List<string> Permissions { get; set; }
}