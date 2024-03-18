import { ServiceRequest } from "../Services/ServiceRequest";

export interface SignUpRequest extends ServiceRequest {
    DisplayName?: string;
    Email?: string;
    Password?: string;
}