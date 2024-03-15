import { Decorators, EntityDialog } from '@serenity-is/corelib';
import { WindowsGroupForm, WindowsGroupRow, WindowsGroupService } from '../../ServerTypes/Administration';

@Decorators.registerClass('WinAuthSample.Administration.WindowsGroupDialog')
export class WindowsGroupDialog extends EntityDialog<WindowsGroupRow, any> {
    protected getFormKey() { return WindowsGroupForm.formKey; }
    protected getIdProperty() { return WindowsGroupRow.idProperty; }
    protected getLocalTextPrefix() { return WindowsGroupRow.localTextPrefix; }
    protected getNameProperty() { return WindowsGroupRow.nameProperty; }
    protected getService() { return WindowsGroupService.baseUrl; }
    protected getDeletePermission() { return WindowsGroupRow.deletePermission; }
    protected getInsertPermission() { return WindowsGroupRow.insertPermission; }
    protected getUpdatePermission() { return WindowsGroupRow.updatePermission; }

    protected form = new WindowsGroupForm(this.idPrefix);
}