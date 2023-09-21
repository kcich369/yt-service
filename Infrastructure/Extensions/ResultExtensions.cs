using Domain.Results;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Extensions;

public static class ResultExtensions
{
    public static Result<T> LogErrorMessage<T, TService>(this Result<T> result, ILogger<TService> logger,
        string applyMessage = null) where TService : class
    {
        if (result.IsError && !string.IsNullOrEmpty(result.ErrorMessage))
            logger.LogError("{ResultErrorMessage}",
                (string.IsNullOrEmpty(applyMessage)
                    ? result.ErrorMessage
                    : string.Join(' ', new { applyMessage, result.ErrorMessage })));
        return result;
    }
}