import { getLookup, getLookupAsync, fieldsProxy } from "@serenity-is/corelib";

export interface WindowsGroupRow {
    Id?: number;
    GroupName?: string;
    Roles?: number[];
}

export abstract class WindowsGroupRow {
    static readonly idProperty = 'Id';
    static readonly nameProperty = 'GroupName';
    static readonly localTextPrefix = 'Membership.WindowsGroup';
    static readonly lookupKey = 'Membership.WindowsGroup';

    /** @deprecated use getLookupAsync instead */
    static getLookup() { return getLookup<WindowsGroupRow>('Membership.WindowsGroup') }
    static async getLookupAsync() { return getLookupAsync<WindowsGroupRow>('Membership.WindowsGroup') }

    static readonly deletePermission = 'Administration:Security';
    static readonly insertPermission = 'Administration:Security';
    static readonly readPermission = 'Administration:Security';
    static readonly updatePermission = 'Administration:Security';

    static readonly Fields = fieldsProxy<WindowsGroupRow>();
}