import { ColumnsBase, fieldsProxy } from "@serenity-is/corelib";
import { Column } from "@serenity-is/sleekgrid";
import { MyProfileRow } from "./MyProfileRow";

export interface MyProfileColumns {
    UserId: Column<MyProfileRow>;
    Username: Column<MyProfileRow>;
    DisplayName: Column<MyProfileRow>;
    FormInitials: Column<MyProfileRow>;
    Email: Column<MyProfileRow>;
    UserImage: Column<MyProfileRow>;
}

export class MyProfileColumns extends ColumnsBase<MyProfileRow> {
    static readonly columnsKey = 'Membership.MyProfile';
    static readonly Fields = fieldsProxy<MyProfileColumns>();
}