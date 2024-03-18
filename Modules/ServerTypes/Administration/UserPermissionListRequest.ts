import { ServiceRequest } from "../Services/ServiceRequest";

export interface UserPermissionListRequest extends ServiceRequest {
    UserID?: number;
}