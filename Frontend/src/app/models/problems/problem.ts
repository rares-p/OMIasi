export interface Problem {
    id: string,
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
    contest: number
}
