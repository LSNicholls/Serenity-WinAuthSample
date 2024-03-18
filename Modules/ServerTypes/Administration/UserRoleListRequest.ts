import { ServiceRequest } from "../Services/ServiceRequest";

export interface UserRoleListRequest extends ServiceRequest {
    UserID?: number;
}