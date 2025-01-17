import { JwtPayload } from "jwt-decode";

export interface JwtInterface extends JwtPayload {
    role: string;
    unique_name: string;
}
