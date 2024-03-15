import { Decorators, EntityGrid, confirm } from '@serenity-is/corelib';
import { WindowsGroupColumns, WindowsGroupRow, WindowsGroupService } from '../../ServerTypes/Administration';
import { WindowsGroupDialog } from './WindowsGroupDialog';

@Decorators.registerClass('WinAuthSample.Administrationp.WindowsGroupGrid')
export class WindowsGroupGrid extends EntityGrid<WindowsGroupRow, any> {
    protected getColumnsKey() { return WindowsGroupColumns.columnsKey; }
    protected getDialogType() { return WindowsGroupDialog; }
    protected getIdProperty() { return WindowsGroupRow.idProperty; }
    protected getInsertPermission() { return WindowsGroupRow.insertPermission; }
    protected getLocalTextPrefix() { return WindowsGroupRow.localTextPrefix; }
    protected getService() { return WindowsGroupService.baseUrl; }

    constructor(container: JQuery) {
        super(container);
    }


    protected getColumns<HelpCategoryColumns>() {
        var columns = super.getColumns();

        columns.unshift({
            field: 'Delete Row',
            name: '',
            format: ctx => '<a class="inline-action delete-row" title="delete network group">' +
                '<i class="fa fa-trash-o text-red"></i></a>',
            width: 24,
            minWidth: 24,
            maxWidth: 24
        });

        columns.splice(1, 0, {
            field: 'Edit Contents',
            name: '',
            format: ctx => `<a class="inline-action edit-details" title="edit network group">
                    <i class="fa fa-edit"></i></a>`,
            width: 24,
            minWidth: 24,
            maxWidth: 24
        });



        return columns;
    }
    protected onClick(e: JQueryEventObject, row: number, cell: number) {
        super.onClick(e, row, cell);

        if (e.isDefaultPrevented())
            return;

        var item = this.itemAt(row);
        var target = $(e.target);

        // if user clicks "i" element, e.g. icon
        if (target.parent().hasClass('inline-action'))
            target = target.parent();

        if (target.hasClass('inline-action')) {
            e.preventDefault();

            if (target.hasClass('delete-row')) {

                confirm('Delete Windows Group [' + item.GroupName.toString() + '] ?', () => {
                    WindowsGroupService.Delete({
                        EntityId: item.Id,
                    }, response => {
                        this.refresh();
                    });
                });

            }
            else if (target.hasClass('edit-details')) {
                // this is under Administration now
                //  if (Authorization.hasPermission("Help:Modify")) {
                this.editItem(item.Id);
                //  }
            }



            ;
        }
    }
}