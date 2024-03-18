import { ServiceRequest } from "../Services/ServiceRequest";

export interface TranslationUpdateRequest extends ServiceRequest {
    TargetLanguageID?: string;
    Translations?: { [key: string]: string };
}