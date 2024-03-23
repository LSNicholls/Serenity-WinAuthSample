import { RoleRow, UserColumns, UserRow, UserService } from "@/ServerTypes/Administration";
import { Decorators, EntityGrid } from "@serenity-is/corelib";
import { Lookup, resolveUrl, tryFirst, confirmDialog } from "@serenity-is/corelib";
import { UserDialog } from "./UserDialog";
import { AdminRefreshUser } from '../../Administration/User/Authentication/AuthUtils';

@Decorators.registerClass()
export class UserGrid extends EntityGrid<UserRow, any> {
    protected getColumnsKey() { return UserColumns.columnsKey; }
    protected getDialogType() { return UserDialog; }
    protected getIdProperty() { return UserRow.idProperty; }
    protected getIsActiveProperty() { return UserRow.isActiveProperty; }
    protected getLocalTextPrefix() { return UserRow.localTextPrefix; }
    protected getService() { return UserService.baseUrl; }

    constructor(container: JQuery) {
        super(container);
    }

    protected override getDefaultSortBy() {
        return [UserRow.Fields.Username];
    }

    protected override createIncludeDeletedButton() {
    }

    //winauth changes -- please note that your method will be
    // different if you are using StartSharp, you will still want to keep
    // some impersonation code that you don't see here:
    protected override getColumns() {
        let columns = super.getColumns();
        let cols = new UserColumns(columns);

        // if you are using StartSharp, here is where
        // the impersonation code should be
        /*
        let impersonationToken = cols.ImpersonationToken;
        if (impersonationToken) {
            impersonationToken.format = ctx => {
                if (!ctx.value)
                    return "";

                return `<a target="_blank" href="${resolveUrl('~/Account/ImpersonateAs?token=')}${ctx.value}">`
                    + `<i class="fa fa-user-secret text-blue"></i></a>`;
            }
        }
        */
        var roles = cols.Roles;
        if (roles) {
            var rolesLookup: Lookup<RoleRow>;
            RoleRow.getLookupAsync().then(lookup => {
                rolesLookup = lookup;
                this.slickGrid.invalidate();
            });

            roles.format = ctx => {
                if (!rolesLookup)
                    return `<i class="fa fa-spinner"></i>`;

                var roleList = (ctx.value || []).map(x => (rolesLookup.itemById[x] || {}).RoleName || "");
                roleList.sort();
                return roleList.join(", ");
            };
        }
        columns.splice(1, 0, {
            field: 'Refresh User',
            name: '',
            format: ctx => `<a class="inline-action recycle-cache text-green" title="refresh user data">
                    <i class="fa fa-recycle"></i></a>`,
            width: 24,
            minWidth: 24,
            maxWidth: 24
        });
        return columns;
    }

    // winauth new methods:
    protected onClick(e: JQueryEventObject, row: number, cell: number) {
        super.onClick(e, row, cell);

        if (e.isDefaultPrevented())
            return;

        var item = this.itemAt(row);
        var target = $(e.target);

        if (target.parent().hasClass('inline-action'))
            target = target.parent();

        if (target.hasClass('inline-action')) {
            e.preventDefault();
            if (target.hasClass('recycle-cache')) {
                this.refreshUserData(item.UserId, item.Username);
            }
        }
    }

    protected refreshUserData(userId, userName) {

        if (userName.indexOf("\\") > 1) {
            var msg = "The user's cached site details have been removed. The user's roles have also been re-queried from Windows (you may not see the results until you refresh this screen).";
            var msg2 = "\n\nIf this user had other roles manually assigned, the additional roles will have been removed and you may need to re-assign some.";
            var msg3 = "\n\nThe user may need to request a refresh via their Profile page, or log out and back in, to see an updated Dashboard in some cases.";


            confirmDialog("PRESERVE existing roles not corresponding\nto Windows network groups\nfor " +
                userName + " (answer 'Yes') ?\n\nAnswering 'No' will REMOVE roles that may have been manually assigned. " +
                "\n\nUse the Close box to cancel.", () => AdminRefreshUser(userId, userName, msg + msg3,false) ,
           {
               onNo: () => AdminRefreshUser(userId, userName, msg + msg2 + msg3, true)
            }
              
            )
        }
        else {
            var msg = "The user's cached site details have been removed.";
            var msg2 = "\n\nThe user may need to request a refresh via their Profile page, or log out and back in, to see an updated Dashboard in some cases.";

            AdminRefreshUser(userId, userName, msg + msg2, false);



        }

    }

}
