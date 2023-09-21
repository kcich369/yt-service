using Domain.Enumerations;
using Domain.Exceptions;
using Domain.Results;
using Microsoft.AspNetCore.Mvc;
using Presentation.ApiResult;
using Presentation.ObjectResult;

namespace Presentation.Extensions;

public static class ResultExtensions
{
    public static async Task<IActionResult> ToActionResult<T>(this Task<IResult<T>> handler)
    {
        var result = await handler;
        var apiResult = new ApiResult<T>(result);
        if (!result.IsError)
            return new OkObjectResult(apiResult);
        if (result.ErrorType!.Contains(ErrorTypesEnums.Validation, ErrorTypesEnums.BadRequest))
            return new BadRequestObjectResult(apiResult);
        if (result.ErrorType == ErrorTypesEnums.NotFound)
            return new NotFoundResult();
        if (result.ErrorType == ErrorTypesEnums.Exception)
            return new InternalServerErrorObjectResult(apiResult);

        throw new InvalidResultException("Invalid result error type");
    }
}