using Energy.Shared.Models.V1.Common.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Energy.Web.Common.Filters;

/// <summary>
/// Birçok JSON uç noktası API'ye vekillik eder ve ham <see cref="BaseResponse{T}"/>
/// zarfını <c>Json(envelope)</c> ile döndürür; bu her zaman HTTP 200 yayar — <c>IsSuccess</c>
/// false olsa bile (ör. yinelenen ad, doğrulama hatası). İstemci tarafındaki <c>AppHttp</c>
/// katmanı yalnızca 2xx olmayan yanıtları başarısızlık olarak değerlendirdiğinden,
/// başarısız bir değişiklik sessizce "kaydedildi" olarak raporlanıyordu.
///
/// Bu filtre, gövdeye dokunmadan başarısız her zarfı <c>400 Bad Request</c>'e yükseltir;
/// böylece mevcut <c>.catch(AppNotify.fromHttpError)</c> işleyicileri gerçek API mesajını
/// yüzeye çıkarır. Başarılı zarflar ve zarf olmayan yükler (grid <c>{ data, totalCount }</c>
/// biçimleri, dosyalar, ...) olduğu gibi bırakılır.
/// </summary>
public sealed class EnvelopeStatusResultFilter : IResultFilter
{
    /// <summary>Sonuç yürütülmeden önce başarısız zarfları 400 durum koduna yükseltir.</summary>
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

    /// <summary>Sonuç yürütüldükten sonra çağrılır (işlem gerektirmez).</summary>
    public void OnResultExecuted(ResultExecutedContext context) { }
}

