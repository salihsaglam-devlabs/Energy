using Energy.Shared.Models.V1.Common.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Energy.Web.Common.Filters;

/// <summary>
/// Many JSON endpoints proxy the API and return the raw <see cref="BaseResponse{T}"/>
/// envelope via <c>Json(envelope)</c>, which always emits HTTP 200 — even when
/// <c>IsSuccess</c> is false (e.g. duplicate name, validation error). The
/// client-side <c>AppHttp</c> layer only treats non-2xx responses as failures,
/// so a failed mutation was silently reported as "saved".
///
/// This filter promotes any failed envelope to <c>400 Bad Request</c> while
/// leaving the body untouched, so the existing <c>.catch(AppNotify.fromHttpError)</c>
/// handlers surface the real API message. Successful envelopes and non-envelope
/// payloads (grid <c>{ data, totalCount }</c> shapes, files, ...) are left as-is.
/// </summary>
public sealed class EnvelopeStatusResultFilter : IResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
        var value = context.Result switch
        {
            JsonResult json => json.Value,
            ObjectResult obj => obj.Value,
            _ => null
        };

        if (value is null) return;

        var type = value.GetType();
        if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(BaseResponse<>))
        {
            return;
        }

        if (type.GetProperty(nameof(BaseResponse<object>.IsSuccess))?.GetValue(value) is not false)
        {
            return;
        }

        switch (context.Result)
        {
            case JsonResult json when json.StatusCode is null or StatusCodes.Status200OK:
                json.StatusCode = StatusCodes.Status400BadRequest;
                break;
            case ObjectResult obj when obj.StatusCode is null or StatusCodes.Status200OK:
                obj.StatusCode = StatusCodes.Status400BadRequest;
                break;
        }
    }

    public void OnResultExecuted(ResultExecutedContext context) { }
}

