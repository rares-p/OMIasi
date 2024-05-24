﻿namespace Domain.Common;

public class Result<T> where T : class
{
    private Result(bool isSuccess, T value, string error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public bool IsSuccess { get; }
    public T Value { get; }
    public string Error { get; }

    public static Result<T> Success(T value) => new(true, value, null!);

    public static Result<T> Failure(string error) => new(false, null!, error);
}