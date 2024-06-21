import { BaseResponse } from "./baseResponse";

export interface TestContentResponse extends BaseResponse {
    input: string,
    output: string
}