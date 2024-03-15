import { fieldsProxy } from "@serenity-is/corelib";

export interface MyProfileRow {
    UserId?: number;
    Username?: string;
    DisplayName?: string;
    Email?: string;
    FormInitials?: string;
    UserImage?: string;
}

export abstract class MyProfileRow {
    static readonly idProperty = 'UserId';
    static readonly nameProperty = 'Username';
    static readonly localTextPrefix = 'Membership.MyProfile';
    static readonly deletePermission = 'None';
    static readonly insertPermission = 'None';
    static readonly readPermission = '*';
    static readonly updatePermission = '*';

    static readonly Fields = fieldsProxy<MyProfileRow>();
}