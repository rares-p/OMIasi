import { SubmissionTestResult } from "./submissionTestResult"

export type SubmissionModel = {
    date: Date,
    id: string,
    scores: SubmissionTestResult[]
}