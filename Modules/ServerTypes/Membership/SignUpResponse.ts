import { ServiceResponse } from "../Services/ServiceResponse";

export interface SignUpResponse extends ServiceResponse {
    DemoActivationLink?: string;
}