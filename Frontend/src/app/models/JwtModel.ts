import { JwtInterface } from "../services/auth/jwt-interface";

export interface JwtModel extends JwtInterface {
    unique_name: string;
  }