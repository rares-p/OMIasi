import { SubmissionModel } from "../submissions/submissionModel";
import { SubmissionTestResult } from "../submissions/submissionTestResult";
import { BaseResponse } from "./baseResponse";

export interface SubmissionResponse extends BaseResponse {
    submissions: SubmissionModel[]
}
