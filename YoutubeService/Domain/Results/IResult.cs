using Domain.Enumerations;

namespace Domain.Results;

public interface IResult
{
    public bool IsError { get; }
    public string ErrorMessage { get; }
    public ErrorTypesEnums ErrorType { get; }
}

public interface IResult<out T> : IResult
{
    public T Data { get; }
}