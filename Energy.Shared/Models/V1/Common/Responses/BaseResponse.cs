using System.Text.Json.Serialization;

namespace Energy.Shared.Models.V1.Common.Responses;

/// <summary>
/// Tüm API yanıtları için standart zarf. Başarı durumunu, mesajı, veriyi ve
/// hata listesini taşır.
/// </summary>
/// <typeparam name="T">Taşınan veri türü.</typeparam>
public class BaseResponse<T>
{
    /// <summary>İşlemin başarılı olup olmadığı.</summary>
    [JsonPropertyName("success")]
    public bool IsSuccess { get; init; }

    /// <summary>Kullanıcıya gösterilecek bilgilendirme mesajı.</summary>
    public string Message { get; init; } = string.Empty;

    /// <summary>Yanıtın veri yükü.</summary>
    public T? Data { get; init; }

    /// <summary>Varsa, hata mesajlarının listesi.</summary>
    public IReadOnlyCollection<string> Errors { get; init; }
        = Array.Empty<string>();

    /// <summary>Başarılı bir yanıt zarfı oluşturur.</summary>
    public static BaseResponse<T> Success(
        T data,
        string message = "")
    {
        return new BaseResponse<T>
        {
            IsSuccess = true,
            Message = message,
            Data = data
        };
    }

    /// <summary>Başarısız bir yanıt zarfı oluşturur.</summary>
    public static BaseResponse<T> Failure(
        string message,
        IEnumerable<string>? errors = null)
    {
        return new BaseResponse<T>
        {
            IsSuccess = false,
            Message = message,
            Errors = errors?.ToArray()
                     ?? Array.Empty<string>()
        };
    }
}
