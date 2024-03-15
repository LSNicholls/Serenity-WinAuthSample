using Serenity.ComponentModel;
using Serenity.Web;
using System.Collections.Generic;
using System.ComponentModel;

namespace WinAuthSample.Administration.Forms
{
    [FormScript("Administration.WindowsGroup")]
    [BasedOnRow(typeof(WindowsGroupRow), CheckNames = true)]
    public class WindowsGroupForm
    {
        [LabelWidth(200)]
        [DisplayName("Windows Group")]
        public string GroupName { get; set; }
        [LabelWidth(200)]
        public List<int> Roles { get; set; }
    }
}