import { ColumnsBase, fieldsProxy } from "@serenity-is/corelib";
import { Column } from "@serenity-is/sleekgrid";
import { WindowsGroupRow } from "./WindowsGroupRow";

export interface WindowsGroupColumns {
    Id: Column<WindowsGroupRow>;
    GroupName: Column<WindowsGroupRow>;
}

export class WindowsGroupColumns extends ColumnsBase<WindowsGroupRow> {
    static readonly columnsKey = 'Administration.WindowsGroup';
    static readonly Fields = fieldsProxy<WindowsGroupColumns>();
}