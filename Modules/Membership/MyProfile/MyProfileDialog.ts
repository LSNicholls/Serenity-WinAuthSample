import { Decorators, EntityDialog, ToolButton, resolveUrl } from '@serenity-is/corelib';
import { MyProfileForm, MyProfileRow, MyProfileService } from '../../ServerTypes/Membership';
import { RemoveCurrentUserCache } from '../../Administration/User/Authentication/AuthUtils';
import { UserPreferenceStorage } from "@serenity-is/extensions";

 

@Decorators.registerClass('WinAuthSample.Membership.MyProfileDialog')
    @Decorators.registerEditor('WinAuthSample.Membership.MyProfileDialog')
 @Decorators.panel()
export class MyProfileDialog extends EntityDialog<MyProfileRow, any> {
    protected getFormKey() { return MyProfileForm.formKey; }
    protected getIdProperty() { return MyProfileRow.idProperty; }
    protected getLocalTextPrefix() { return MyProfileRow.localTextPrefix; }
    protected getNameProperty() { return MyProfileRow.nameProperty; }
    protected getService() { return MyProfileService.baseUrl; }
    protected getDeletePermission() { return MyProfileRow.deletePermission; }
    protected getInsertPermission() { return MyProfileRow.insertPermission; }
    protected getUpdatePermission() { return MyProfileRow.updatePermission; }

    protected form = new MyProfileForm(this.idPrefix);
    protected archivePrivs: boolean = false;
    protected archiving: boolean = false;
 

    
    protected updateInterface(): void {
 
        super.updateInterface();
        this.toolbar.findButton('delete-button').hide();
      
        
     
    }

    protected getToolbarButtons(): ToolButton[] {
        var buttons = super.getToolbarButtons();
        let newBtn = this.createToolButtonRecycle();
        buttons.push(newBtn);
        
    return buttons;
    }

   

    
   
    protected createToolButtonRecycle() {
        return <ToolButton>{
            separator: true,
            title: 'Refresh…',
            icon: 'fa-recycle text-green',
            onClick: () => {
                this.doUserRefresh();
            }
        };
    }

    protected doUserRefresh() {

        // TBD -- can we check to see if the current identity is the same as the form identity.  
        //If not, we are impersonating and can make a decision about
        // whether or not we want to remove other roles besides those that are allowed by role.
        // Ask the (admin) user whether they want to do this and pass along the information.
        // (Right now that param of the procedure isn't even exposed, because it won't make 
        // sense to use it unless we are able to determine whether the current user is impersonating)
        

        var msg = "Your cached details have been removed.\n\n" +
            ((this.form.Username.value.indexOf("\\") > 1) ? "If you have recently become a member of a relevant Windows network group,\nadditional application roles may have been added.\n\n" : "") +
            "Try going back to the Dashboard page by clicking on it in the menu. \nIf still needed, refresh your browser with Ctrl-F5.\n\n" +
            "If that doesn't work, using one of the\nLogout methods, then logging back in, and then \nre-refreshing the browser should do it.";

        // note: the "YES" value is currently being ignored, we treat the existance of the row for a user as a flag.
        // This preference can be set anywhere in the application where you think a change is happening that should
        // immediately cause a refresh of the Dashboard for this particular user.  Typically this won't matter to
        // anything but user-specific details, and those would be specific to a given application, so the usage in
        // the Dashboard for this sample app is illustrative, but it seems reasonable to set the flag when the user's
        // standard cached details -- whatever they are -- are requested to be refreshed.
        new UserPreferenceStorage().setItem("DashboardNeedsRefresh", "YES");

        RemoveCurrentUserCache(this.entityId, this.form.Username.value, msg, true);
        
    }
    protected onDialogClose() {
        super.onDialogClose();
        window.location.href= resolveUrl("~/");
    }
}