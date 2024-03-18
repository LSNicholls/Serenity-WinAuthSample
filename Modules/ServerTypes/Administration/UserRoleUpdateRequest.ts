import { ServiceRequest } from "../Services/ServiceRequest";

export interface UserRoleUpdateRequest extends ServiceRequest {
    UserID?: number;
    Roles?: number[];
}