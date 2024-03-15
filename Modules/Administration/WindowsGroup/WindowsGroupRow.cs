using Serenity.ComponentModel;
using Serenity.Data;
using Serenity.Data.Mapping;
using System.ComponentModel;
using System.Collections.Generic;

namespace WinAuthSample.Administration
{
    [ConnectionKey("Default"), Module("Membership"), TableName("[dbo].[WindowsGroup]")]
    [DisplayName("Windows Group"), InstanceName("Windows Group")]
    [ReadPermission("Administration:Security")]
    [ModifyPermission("Administration:Security")]

    [LookupScript]
    public sealed class WindowsGroupRow : Row<WindowsGroupRow.RowFields>, IIdRow, INameRow
    {
        [DisplayName("Id"), Identity, IdProperty, Hidden]
        public int? Id
        {
            get => fields.Id[this];
            set => fields.Id[this] = value;
        }

        [DisplayName("Group Name - associate to Roles"), Size(100), NotNull, QuickSearch, NameProperty]
        public string GroupName
        {
            get => fields.GroupName[this];
            set => fields.GroupName[this] = value;
        }
        [DisplayName("Associated with Roles"), LinkingSetRelation(typeof(WindowsGroupRolesRow), nameof(WindowsGroupRolesRow.WindowsGroupId), nameof(WindowsGroupRolesRow.RoleId))]
        [LookupEditor(typeof(Administration.RoleRow), Multiple = true)]
        public List<int> Roles
        {
            get => fields.Roles[this];
            set => fields.Roles[this] = value;
        }
        public WindowsGroupRow()
            : base()
        {
        }

        public WindowsGroupRow(RowFields fields)
            : base(fields)
        {
        }

        public class RowFields : RowFieldsBase
        {
            public Int32Field Id;
            public StringField GroupName;
            public ListField<int> Roles;
        }
    }
}