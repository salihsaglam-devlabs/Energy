using Energy.Shared.Models.V1.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Web.Common;

/// <summary>
/// API tarafından döndürülen <see cref="BaseResponse{T}"/> zarflarını, DevExtreme
/// istemcisinin beklediği biçimlere köprüler.
/// </summary>
public static class ApiResponseExtensions
{
    /// <summary>
    /// <c>CustomStore</c> ile <c>dxDataGrid</c>'in tükettiği <c>{ data, totalCount }</c>
    /// biçimini döndürür. Başarısızlıkta, API hata mesajını taşıyan 400 JSON yanıtı döndürür.
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
    /// API yükünü JSON olarak veya başarısızlıkta API hata mesajıyla bir 400 olarak
    /// döndürür — API'nin döndürdüğü HTTP durumundan bağımsızdır.
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

