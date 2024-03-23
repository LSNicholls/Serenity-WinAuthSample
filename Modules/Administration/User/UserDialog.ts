import { UserForm, UserRow, UserService } from "../";
import { UserPermissionDialog } from "../UserPermission/UserPermissionDialog";
import { Decorators, EditorUtils, EntityDialog, confirmDialog } from "@serenity-is/corelib"
import { format, localText } from "@serenity-is/corelib";
import { Texts } from "@/ServerTypes/Texts";
import { AdminRefreshUser } from '../../Administration/User/Authentication/AuthUtils';

@Decorators.registerClass()
export class UserDialog extends EntityDialog<UserRow, any> {
    protected getFormKey() { return UserForm.formKey; }
    protected getIdProperty() { return UserRow.idProperty; }
    protected getIsActiveProperty() { return UserRow.isActiveProperty; }
    protected getLocalTextPrefix() { return UserRow.localTextPrefix; }
    protected getNameProperty() { return UserRow.nameProperty; }
    protected getService() { return UserService.baseUrl; }

    protected form = new UserForm(this.idPrefix);

    constructor() {
        super();

        this.form.Password.change(() => {
            EditorUtils.setRequired(this.form.PasswordConfirm, this.form.Password.value.length > 0);
        });

        this.form.Password.addValidationRule(this.uniqueName, e => {
            if (this.form.Password.value.length < 6)
                return format(localText(Texts.Validation.MinRequiredPasswordLength), 6);
        });

        this.form.PasswordConfirm.addValidationRule(this.uniqueName, e => {
            if (this.form.Password.value != this.form.PasswordConfirm.value)
                return localText(Texts.Validation.PasswordConfirmMismatch);
        });
    }

    protected getToolbarButtons()
    {
        let buttons = super.getToolbarButtons();

        buttons.push({
            title: localText(Texts.Site.UserDialog.EditPermissionsButton),
            cssClass: 'edit-permissions-button',
            icon: 'fa-lock text-green',
            onClick: () =>
            {
                new UserPermissionDialog({
                    userID: this.entity.UserId,
                    username: this.entity.Username
                }).dialogOpen();
            }
        });
    

        //winauth additions:
        buttons.splice(2, 0, {
            title: "",
            hint: "Refresh",
            icon: 'fa fa-refresh text-blue',
            cssClass: 'refresh-user-dialog',
            separator: false,
            onClick: () => {
                this.reloadById();
            }
        });

        buttons.push({
            title: localText('Refresh User Data'),
            icon: 'fa-recycle text-green',
            cssClass: 'refresh-user-button',
            separator: true,
            onClick: () => {
                this.refreshUserData();
                
            }
        });
        return buttons;
    }

    protected updateInterface() {
        super.updateInterface();

        this.toolbar.findButton("edit-permissions-button").toggleClass("disabled", this.isNewOrDeleted());
    }

    protected afterLoadEntity() {
        super.afterLoadEntity();
        // winauth additions:

        if (this.form.Source.value.toLowerCase() == "win") {
            // adapt some items for Win auth
            // if available to  you: this.form.TwoFactorAuth.set_readOnly(true);
            EditorUtils.setReadOnly(this.form.Username, true);

            this.form.Password.getGridField().hide();
            this.form.PasswordConfirm.getGridField().hide();

        }
        else {

            // these fields are only required in new record mode
            this.form.Password.element.toggleClass('required', this.isNew())
                .closest('.field').find('sup').toggle(this.isNew());
            this.form.PasswordConfirm.element.toggleClass('required', this.isNew())
                .closest('.field').find('sup').toggle(this.isNew());
        }
    }

    // winauth new method:

    protected refreshUserData() {

        if (this.entity.Username.indexOf("\\") > 1) {
            var msg = "The user's cached site details have been removed. The user's roles have also been re-queried from Windows (you may not see the results until you refresh this screen).";
            var msg2 = "\n\nIf this user had other roles manually assigned, the additional roles will have been removed and you may need to re-assign some.";
            var msg3 = "\n\nThe user may need to request a refresh via their Profile page, or log out and back in, to see an updated Dashboard in some cases.";


            confirmDialog("PRESERVE existing roles not corresponding\nto Windows network groups\nfor " +
                this.entity.Username + " (answer 'Yes') ?\n\nAnswering 'No' will REMOVE roles that may have been manually assigned. " +
                "\n\nUse the Close box to cancel.", () =>
                AdminRefreshUser(this.entityId, this.entity.Username, msg + msg3, false), {
                onNo: () => AdminRefreshUser(this.entityId, this.entity.Username, msg + msg2 + msg3, true)
            })
            
        }
        else {
            var msg = "The user's cached site details have been removed.";
            var msg2 = "\n\nThe user may need to request a refresh via their Profile page, or log out and back in, to see an updated Dashboard in some cases.";

            AdminRefreshUser(this.entityId, this.entity.Username, msg + msg2, false);



        }



    }
}
