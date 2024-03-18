import { ServiceRequest } from "../Services/ServiceRequest";

export interface LoginRequest extends ServiceRequest {
    Username?: string;
    Password?: string;
}