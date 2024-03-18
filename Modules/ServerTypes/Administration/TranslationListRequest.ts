import { ListRequest } from "../Services/ListRequest";

export interface TranslationListRequest extends ListRequest {
    SourceLanguageID?: string;
    TargetLanguageID?: string;
}