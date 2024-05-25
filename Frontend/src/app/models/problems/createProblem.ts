import { problemTest } from "./problemTest"

export type CreateProblem = {
    title: string,
    description: string,
    noTests: number,
    author: string,
    timeLimitInSeconds: number,
    totalMemoryLimitInMb: number,
    stackMemoryLimitInMb: number,
    grade: number,
    inputFileName: string,
    outputFileName: string
    year: number,
    tests: problemTest[]
}