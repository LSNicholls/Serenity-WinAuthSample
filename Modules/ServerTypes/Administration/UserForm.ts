import { PrefixedContext } from "@serenity-is/corelib";

export interface UserForm {
}

export class UserForm extends PrefixedContext {
    static readonly formKey = 'Administration.User';
}