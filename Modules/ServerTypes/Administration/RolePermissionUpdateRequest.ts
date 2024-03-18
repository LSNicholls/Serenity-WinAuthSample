import { ServiceRequest } from "../Services/ServiceRequest";

export interface RolePermissionUpdateRequest extends ServiceRequest {
    RoleID?: number;
    Permissions?: string[];
}