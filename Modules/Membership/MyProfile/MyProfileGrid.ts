import { Decorators, EntityGrid, ToolButton, indexOf} from '@serenity-is/corelib';
import { MyProfileColumns, MyProfileRow, MyProfileService } from '../../ServerTypes/Membership';
import { MyProfileDialog } from './MyProfileDialog';
import { RemoveCurrentUserCache } from '../../Administration/User/Authentication/AuthUtils';
import { UserPreferenceStorage } from "@serenity-is/extensions";

@Decorators.registerClass('WinAuthSample.Membership.MyProfileGrid')
export class MyProfileGrid extends EntityGrid<MyProfileRow, any> {
    protected getColumnsKey() { return MyProfileColumns.columnsKey; }
    protected getDialogType() { return MyProfileDialog; }
    protected getIdProperty() { return MyProfileRow.idProperty; }
    protected getInsertPermission() { return MyProfileRow.insertPermission; }
    protected getLocalTextPrefix() { return MyProfileRow.localTextPrefix; }
    protected getService() { return MyProfileService.baseUrl; }

    constructor(container: JQuery) {
        super(container);
    }

    
    protected getButtons(): ToolButton[] {

       // var buttons  = super.getButtons();
       //buttons.splice(indexOf(buttons, x => x.cssClass == "add-button"), 1);
    
        return null;
    }

    protected getColumns<MyProfileColumns>() {
        var cols = super.getColumns();
        cols.splice(1, 0, {
            field: 'Edit Contents',
            name: '',
            format: ctx => `<a class="inline-action edit-details" title="edit profile in form">
                    <i class="fa fa-edit"></i></a>`,
            width: 24,
            minWidth: 24,
            maxWidth: 24
        });

        cols.splice(1, 0, {
            field: 'Refresh User Cache',
            name: '',
            format: ctx => `<a class="inline-action recycle-cache  text-green" title="refresh user cache">
                    <i class="fa fa-recycle"></i></a>`,
            width: 24,
            minWidth: 24,
            maxWidth: 24
        });
        return cols;
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
            if (target.hasClass('edit-details')) {
                var dlg = new MyProfileDialog();
                dlg.loadByIdAndOpenDialog(item.UserId);
                this.initDialog(dlg);

            }
            else if (target.hasClass('recycle-cache')) {
                var msg = "Your cached details have been removed.\n\n" +
                    ((item.Username.indexOf("\\") > 1) ? "If you have recently become a member of a relevant Windows network group,\nadditional application roles may have been added.\n\n" : "") +
                "Try going back to the Dashboard page by clicking on it in the menu. \nIf still needed, refresh your browser with Ctrl-F5.\n\n" +
                    "If that doesn't work, using one of the\nLogout methods, then logging back in, and then \nre-refreshing the browser should do it."

                // note: the "YES" value is currently being ignored, we treat the existance of the row for a user as a flag.
                // This preference can be set anywhere in the application where you think a change is happening that should
                // immediately cause a refresh of the Dashboard for this particular user.  Typically this won't matter to
                // anything but user-specific details, and those would be specific to a given application, so the usage in
                // the Dashboard for this sample app is illustrative, but it seems reasonable to set the flag when the user's
                // standard cached details -- whatever they are -- are requested to be refreshed.
                new UserPreferenceStorage().setItem("DashboardNeedsRefresh", "YES");

                RemoveCurrentUserCache(item.UserId, item.Username, msg, true);
                
            }
       
           

            ;
        }
    }

}