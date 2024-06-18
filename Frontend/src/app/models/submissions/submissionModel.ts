import { SubmissionTestResult } from "./submissionTestResult"

export type SubmissionModel = {
    id: string,
    solution: string,
    date: Date,
    scores: SubmissionTestResult[]
}