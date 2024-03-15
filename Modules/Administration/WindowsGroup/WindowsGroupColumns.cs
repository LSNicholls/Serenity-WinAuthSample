using Serenity.ComponentModel;
using System.ComponentModel;

namespace WinAuthSample.Administration.Columns
{
    [ColumnsScript("Administration.WindowsGroup")]
    [BasedOnRow(typeof(WindowsGroupRow), CheckNames = true)]
    public class WindowsGroupColumns
    {
        [EditLink, DisplayName("Db.Shared.RecordId"), AlignRight]
        public int Id { get; set; }
        [EditLink]
        public string GroupName { get; set; }
    }
}