import { StringEditor, LookupEditor, PrefixedContext, initFormType } from "@serenity-is/corelib";

export interface WindowsGroupForm {
    GroupName: StringEditor;
    Roles: LookupEditor;
}

export class WindowsGroupForm extends PrefixedContext {
    static readonly formKey = 'Administration.WindowsGroup';
    private static init: boolean;

    constructor(prefix: string) {
        super(prefix);

        if (!WindowsGroupForm.init)  {
            WindowsGroupForm.init = true;

            var w0 = StringEditor;
            var w1 = LookupEditor;

            initFormType(WindowsGroupForm, [
                'GroupName', w0,
                'Roles', w1
            ]);
        }
    }
}