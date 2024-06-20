import { UserProfile } from "../user/userProfile";
import { BaseResponse } from "./baseResponse";

export interface UserProfileResponse extends BaseResponse, UserProfile {
}
