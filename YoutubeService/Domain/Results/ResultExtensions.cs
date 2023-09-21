using Domain.Enumerations;

namespace Domain.Results;

public static class ResultExtensions
{
    public static async Task<IResult<T>> TryCatch<T>(this Task<IResult<T>> task)
    {
        try
        {
            var result = await task;
            return result.IsError ? Result<T>.Error(result) : Result<T>.Success(result.Data);
        }
        catch (Exception e)
        {
            return Result<T>.Error(ErrorTypesEnums.Exception, e.Message);
        }
    }
    
    public static async Task<IResult<T>> TryCatch<T>(this Task<T> task)
    {
        try
        {
            return Result<T>.Success(await task);
        }
        catch (Exception e)
        {
            return Result<T>.Error(ErrorTypesEnums.Exception, e.Message);
        }
    }
    
    public static async Task<IResult<bool>> TryCatch(this Task task)
    {
        try
        {
            await task;
            return Result<bool>.Success(true);
        }
        catch (Exception e)
        {
            return Result<bool>.Error(ErrorTypesEnums.Exception, e.Message);
        }
    }

    public static async ValueTask<IResult<T>> TryCatch<T>(this ValueTask<IResult<T>> task)
    {
        try
        {
            var result = await task;
            return result.IsError ? Result<T>.Error(result) : Result<T>.Success(result.Data);
        }
        catch (Exception e)
        {
            return Result<T>.Error(ErrorTypesEnums.Exception, e.Message);
        }
    }

    public static async Task<IResult<TOut>> Next<TIn, TOut>(this Task<IResult<TIn>> task,
        Func<IResult<TIn>, Task<IResult<TOut>>> next) => await Next(await TryCatch(task), next);

    public static async ValueTask<IResult<TOut>> Next<TIn, TOut>(this ValueTask<IResult<TIn>> task,
        Func<IResult<TIn>, ValueTask<IResult<TOut>>> next) => await Next(await TryCatch(task), next);

    public static async ValueTask<IResult<TOut>> Next<TIn, TOut>(this IResult<TIn> result,
        Func<IResult<TIn>, ValueTask<IResult<TOut>>> next) =>
        result.IsError
            ? Result<TOut>.Error(result)
            : await TryCatch(next.Invoke(result));

    public static async Task<IResult<TOut>> Next<TIn, TOut>(this IResult<TIn> result,
        Func<IResult<TIn>, Task<IResult<TOut>>> next) =>
        result.IsError
            ? Result<TOut>.Error(result)
            : await TryCatch(next.Invoke(result));

    public static IResult<TOut> Next<TIn, TOut>(this IResult<TIn> result,
        Func<IResult<TIn>, IResult<TOut>> next) =>
        result.IsError
            ? Result<TOut>.Error(result)
            : next.Invoke(result);

    public static async Task<IResult<TOut>> Next<TIn, TOut>(this Task<IResult<TIn>> task,
        Func<IResult<TIn>, IResult<TOut>> next)
    {
        var result = await TryCatch(task);
        return result.IsError ? Result<TOut>.Error(result) : next.Invoke(result);
    }
    
    public static IResult<TIn> ReturnOut<TIn>(this IResult<TIn> operationResult, out IResult<TIn> result)
    {
        result = operationResult;
        return operationResult;
    }
}