import { ServiceRequest } from "../Services/ServiceRequest";

export interface RolePermissionListRequest extends ServiceRequest {
    RoleID?: number;
}