import { fieldsProxy } from "@serenity-is/corelib";

export interface WindowsGroupRolesRow {
    Id?: number;
    WindowsGroupId?: number;
    RoleId?: number;
    WindowsGroupName?: string;
    RoleRoleName?: string;
    RoleRoleKey?: string;
}

export abstract class WindowsGroupRolesRow {
    static readonly idProperty = 'Id';
    static readonly localTextPrefix = 'Membership.WindowsGroupRoles';
    static readonly deletePermission = 'Administration:Security';
    static readonly insertPermission = 'Administration:Security';
    static readonly readPermission = 'Administration:Security';
    static readonly updatePermission = 'Administration:Security';

    static readonly Fields = fieldsProxy<WindowsGroupRolesRow>();
}