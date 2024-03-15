import { StringEditor, EmailAddressEditor, ImageUploadEditor, PrefixedContext, initFormType } from "@serenity-is/corelib";

export interface MyProfileForm {
    DisplayName: StringEditor;
    FormInitials: StringEditor;
    Email: EmailAddressEditor;
    UserImage: ImageUploadEditor;
    Username: StringEditor;
}

export class MyProfileForm extends PrefixedContext {
    static readonly formKey = 'Membership.MyProfile';
    private static init: boolean;

    constructor(prefix: string) {
        super(prefix);

        if (!MyProfileForm.init)  {
            MyProfileForm.init = true;

            var w0 = StringEditor;
            var w1 = EmailAddressEditor;
            var w2 = ImageUploadEditor;

            initFormType(MyProfileForm, [
                'DisplayName', w0,
                'FormInitials', w0,
                'Email', w1,
                'UserImage', w2,
                'Username', w0
            ]);
        }
    }
}