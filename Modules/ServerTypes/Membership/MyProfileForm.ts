import { PrefixedContext } from "@serenity-is/corelib";

export interface MyProfileForm {
}

export class MyProfileForm extends PrefixedContext {
    static readonly formKey = 'Membership.MyProfile';
}