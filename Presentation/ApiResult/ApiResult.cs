using Domain.Results;

namespace Presentation.ApiResult;

public class ApiResult<T>
{
    public string ErrorMessage { get; private set; }
    public T Data { get; private set; }

    public ApiResult(IResult<T> result)
    {
        ErrorMessage = result.ErrorMessage;
        Data = result.Data;
    }
}