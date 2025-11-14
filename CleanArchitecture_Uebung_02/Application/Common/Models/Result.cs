namespace Application.Common.Models;

/// <summary>
/// Repräsentiert das Ergebnis einer Operation mit Wert und Status.
/// </summary>
public class Result<T>
{
    public T? Value { get; init; }
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public ResultStatus Status { get; init; }

    public static Result<T> Success(T value) => new()
    {
        Value = value,
        IsSuccess = true,
        Status = ResultStatus.Ok
    };

    public static Result<T> Created(T value) => new()
    {
        Value = value,
        IsSuccess = true,
        Status = ResultStatus.Created
    };

    public static Result<T> NotFound(string message) => new()
    {
        IsSuccess = false,
        ErrorMessage = message,
        Status = ResultStatus.NotFound
    };

    public static Result<T> ValidationError(string message) => new()
    {
        IsSuccess = false,
        ErrorMessage = message,
        Status = ResultStatus.ValidationError
    };

    public static Result<T> Conflict(string message) => new()
    {
        IsSuccess = false,
        ErrorMessage = message,
        Status = ResultStatus.Conflict
    };

    public static Result<T> Error(string message) => new()
    {
        IsSuccess = false,
        ErrorMessage = message,
        Status = ResultStatus.Error
    };
}

public class Result
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public ResultStatus Status { get; init; }

    public static Result Success() => new()
    {
        IsSuccess = true,
        Status = ResultStatus.Ok
    };

    public static Result NotFound(string message) => new()
    {
        IsSuccess = false,
        ErrorMessage = message,
        Status = ResultStatus.NotFound
    };

    public static Result ValidationError(string message) => new()
    {
        IsSuccess = false,
        ErrorMessage = message,
        Status = ResultStatus.ValidationError
    };

    public static Result Conflict(string message) => new()
    {
        IsSuccess = false,
        ErrorMessage = message,
        Status = ResultStatus.Conflict
    };

    public static Result Error(string message) => new()
    {
        IsSuccess = false,
        ErrorMessage = message,
        Status = ResultStatus.Error
    };
}

public enum ResultStatus
{
    Ok,
    Created,
    NotFound,
    ValidationError,
    Conflict,
    Error
}

