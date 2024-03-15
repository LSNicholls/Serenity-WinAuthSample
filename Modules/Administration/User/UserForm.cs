namespace WinAuthSample.Administration.Forms;

[FormScript("Administration.User")]
[BasedOnRow(typeof(UserRow), CheckNames = true)]
public class UserForm
{
    [LabelWidth(200, UntilNext = true)]

    public bool IsActive { get; set; }

    [ThreeQuarterWidth]
    public string Username { get; set; }

    [ThreeQuarterWidth,Size(4), MaxLength(4)]
    public string FormInitials { get; set; }
    public string DisplayName { get; set; }
    [EmailAddressEditor]
    public string Email { get; set; }
    [LookupEditor(typeof(RoleRow), Multiple = true)]
    public List<int> Roles { get; set; }
    public string UserImage { get; set; }
    [PasswordEditor, Required(true)]
    public string Password { get; set; }
    [PasswordEditor, Required(true)]
    public string PasswordConfirm { get; set; }
    [OneWay]
    public string Source { get; set; }
  
}