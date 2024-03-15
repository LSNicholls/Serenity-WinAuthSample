import { SaveRequest, SaveResponse, ServiceOptions, RetrieveRequest, RetrieveResponse, ListRequest, ListResponse, serviceRequest } from "@serenity-is/corelib";
import { MyProfileRow } from "./MyProfileRow";

export namespace MyProfileService {
    export const baseUrl = 'Membership/MyProfile';

    export declare function Create(request: SaveRequest<MyProfileRow>, onSuccess?: (response: SaveResponse) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function Update(request: SaveRequest<MyProfileRow>, onSuccess?: (response: SaveResponse) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function Retrieve(request: RetrieveRequest, onSuccess?: (response: RetrieveResponse<MyProfileRow>) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function List(request: ListRequest, onSuccess?: (response: ListResponse<MyProfileRow>) => void, opt?: ServiceOptions<any>): JQueryXHR;

    export const Methods = {
        Create: "Membership/MyProfile/Create",
        Update: "Membership/MyProfile/Update",
        Retrieve: "Membership/MyProfile/Retrieve",
        List: "Membership/MyProfile/List"
    } as const;

    [
        'Create', 
        'Update', 
        'Retrieve', 
        'List'
    ].forEach(x => {
        (<any>MyProfileService)[x] = function (r, s, o) {
            return serviceRequest(baseUrl + '/' + x, r, s, o);
        };
    });
}