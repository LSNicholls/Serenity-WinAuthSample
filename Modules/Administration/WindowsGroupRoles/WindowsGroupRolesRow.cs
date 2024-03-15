using Serenity.ComponentModel;
using Serenity.Data;
using Serenity.Data.Mapping;
using System.ComponentModel;

namespace WinAuthSample.Administration
{
    [ConnectionKey("Default"), Module("Membership"), TableName("[dbo].[WindowsGroupRoles]")]
    [DisplayName("Windows Group Roles"), InstanceName("Windows Group Role")]
    [ReadPermission("Administration:Security")]
    [ModifyPermission("Administration:Security")]


    public sealed class WindowsGroupRolesRow : Row<WindowsGroupRolesRow.RowFields>, IIdRow
    {
        [DisplayName("Id"), Identity, IdProperty, AlignCenter,Hidden]
        public int? Id
        {
            get => fields.Id[this];
            set => fields.Id[this] = value;
        }

        [DisplayName("Windows Group"), NotNull, ForeignKey("[dbo].[WindowsGroup]", "Id"), LeftJoin("jWindowsGroup"), TextualField("WindowsGroupName")]
        [Width(100), LookupEditor(typeof(WindowsGroupRow))]
        public int? WindowsGroupId
        {
            get => fields.WindowsGroupId[this];
            set => fields.WindowsGroupId[this] = value;
        }

        [DisplayName("Role"), NotNull, ForeignKey("[dbo].[Roles]", "RoleId"), LeftJoin("jRole"), TextualField("Application RoleName")]
        [Width(100), LookupEditor(typeof(Administration.RoleRow))]
        public int? RoleId
        {
            get => fields.RoleId[this];
            set => fields.RoleId[this] = value;
        }

        [DisplayName("Group Name"), Expression("jWindowsGroup.[GroupName]")]
        public string WindowsGroupName
        {
            get => fields.WindowsGroupName[this];
            set => fields.WindowsGroupName[this] = value;
        }

        [DisplayName("Role Name"), Expression("jRole.[RoleName]")]
        public string RoleRoleName
        {
            get => fields.RoleRoleName[this];
            set => fields.RoleRoleName[this] = value;
        }

        [DisplayName("Role Key"), Expression("jRole.[RoleKey]")]
        public string RoleRoleKey
        {
            get => fields.RoleRoleKey[this];
            set => fields.RoleRoleKey[this] = value;
        }

        public WindowsGroupRolesRow()
            : base()
        {
        }

        public WindowsGroupRolesRow(RowFields fields)
            : base(fields)
        {
        }

        public class RowFields : RowFieldsBase
        {
            public Int32Field Id;
            public Int32Field WindowsGroupId;
            public Int32Field RoleId;

            public StringField WindowsGroupName;

            public StringField RoleRoleName;
            public StringField RoleRoleKey;
        }
    }
}