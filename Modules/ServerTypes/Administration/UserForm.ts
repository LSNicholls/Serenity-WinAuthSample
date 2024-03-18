import { BooleanEditor, StringEditor, EmailAddressEditor, LookupEditor, ImageUploadEditor, PasswordEditor, PrefixedContext, initFormType } from "@serenity-is/corelib";

export interface UserForm {
    IsActive: BooleanEditor;
    Username: StringEditor;
    FormInitials: StringEditor;
    DisplayName: StringEditor;
    Email: EmailAddressEditor;
    Roles: LookupEditor;
    UserImage: ImageUploadEditor;
    Password: PasswordEditor;
    PasswordConfirm: PasswordEditor;
    Source: StringEditor;
}

export class UserForm extends PrefixedContext {
    static readonly formKey = 'Administration.User';
    private static init: boolean;

    constructor(prefix: string) {
        super(prefix);

        if (!UserForm.init)  {
            UserForm.init = true;

            var w0 = BooleanEditor;
            var w1 = StringEditor;
            var w2 = EmailAddressEditor;
            var w3 = LookupEditor;
            var w4 = ImageUploadEditor;
            var w5 = PasswordEditor;

            initFormType(UserForm, [
                'IsActive', w0,
                'Username', w1,
                'FormInitials', w1,
                'DisplayName', w1,
                'Email', w2,
                'Roles', w3,
                'UserImage', w4,
                'Password', w5,
                'PasswordConfirm', w5,
                'Source', w1
            ]);
        }
    }
}