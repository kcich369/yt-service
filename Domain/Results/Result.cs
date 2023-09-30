using Domain.Enumerations;

namespace Domain.Results;

public class Result : IResult
{
    public bool IsError { get; private set; }
    public string ErrorMessage { get; private set; }
    public ErrorTypesEnums ErrorType { get; private set; }

    protected Result()
    {
    }

    protected Result(ErrorTypesEnums errorTypesEnums, string errorMessage)
    {
        ErrorType = errorTypesEnums;
        ErrorMessage = errorMessage;
        IsError = true;
    }
}

public class Result<T> : Result, IResult<T>
{
    public T Data { get; private set; }

    private Result(T data)
    {
        Data = data;
    }

    private Result(ErrorTypesEnums errorTypesEnums, string errorMessage) : base(errorTypesEnums, errorMessage)
    {
    }

    public static Result<T> Success(T data) => new Result<T>(data);
    
    public static Result<T> Error(IResult errorResult)
    {
        if (!errorResult.IsError)
            throw new ArgumentException("Data is not error");
        return new Result<T>(errorResult.ErrorType, errorResult.ErrorMessage);
    }

    public static Result<T> Error(ErrorTypesEnums errorTypesEnums, string errorMessage = null) =>
        new Result<T>(errorTypesEnums, errorMessage);
}