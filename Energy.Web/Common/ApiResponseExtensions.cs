using Energy.Shared.Models.V1.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Web.Common;

/// <summary>
/// Bridges <see cref="BaseResponse{T}"/> envelopes returned by the API into
/// shapes the DevExtreme client expects.
/// </summary>
public static class ApiResponseExtensions
{
    /// <summary>
    /// Returns the <c>{ data, totalCount }</c> shape consumed by
    /// <c>dxDataGrid</c> with <c>CustomStore</c>. On failure, returns a 400
    /// JSON response carrying the API error message.
    /// </summary>
    public static IActionResult ToGridResult<T>(
        this BaseResponse<PaginatedResponse<T>> envelope)
    {
        if (!envelope.IsSuccess || envelope.Data is null)
        {
            return new BadRequestObjectResult(new
            {
                message = envelope.Message,
                errors = envelope.Errors
            });
        }

        return new OkObjectResult(new
        {
            data = envelope.Data.Items,
            totalCount = envelope.Data.TotalCount
        });
    }

    /// <summary>
    /// Returns the API payload as JSON or, on failure, a 400 with the API
    /// error message — independent of the HTTP status the API itself returned.
    /// </summary>
    public static IActionResult ToJsonResult<T>(this BaseResponse<T> envelope)
    {
        if (!envelope.IsSuccess)
        {
            return new BadRequestObjectResult(new
            {
                message = envelope.Message,
                errors = envelope.Errors
            });
        }

        return new OkObjectResult(new
        {
            data = envelope.Data,
            message = envelope.Message
        });
    }
}

