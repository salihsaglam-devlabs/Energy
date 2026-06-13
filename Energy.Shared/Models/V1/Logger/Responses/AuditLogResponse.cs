namespace Energy.Shared.Models.V1.Logger.Responses;

/// <summary>Tek bir denetim (audit) günlüğü kaydının istemciye sunulan görünümü.</summary>
public sealed class AuditLogResponse
{
    /// <summary>Kaydın benzersiz kimliği.</summary>
    public long Id { get; init; }

    /// <summary>Olayın gerçekleştiği zaman.</summary>
    public DateTime OccurredAt { get; init; }

    /// <summary>İlgili kullanıcının kimliği (anonimse null).</summary>
    public Guid? UserId { get; init; }

    /// <summary>İlgili kullanıcının adı.</summary>
    public string? UserName { get; init; }

    /// <summary>İstemci IP adresi.</summary>
    public string? IpAddress { get; init; }

    /// <summary>HTTP metodu.</summary>
    public string? HttpMethod { get; init; }

    /// <summary>İstek yolu.</summary>
    public string? Path { get; init; }

    /// <summary>İsteğin sorgu dizesi.</summary>
    public string? QueryString { get; init; }

    /// <summary>Yanıtın HTTP durum kodu.</summary>
    public int StatusCode { get; init; }

    /// <summary>İsteğin başarılı olup olmadığı.</summary>
    public bool IsSuccess { get; init; }

    /// <summary>Kaydın kaynağı (örn. API, Web).</summary>
    public string? Source { get; init; }

    /// <summary>İstek gövdesi (maskelenmiş).</summary>
    public string? RequestBody { get; init; }

    /// <summary>Yanıt gövdesi (maskelenmiş).</summary>
    public string? ResponseBody { get; init; }

    /// <summary>İstek sırasında istisna oluşup oluşmadığı.</summary>
    public bool HasException { get; init; }

    /// <summary>İstisnanın tür adı.</summary>
    public string? ExceptionType { get; init; }

    /// <summary>İstisnanın mesajı.</summary>
    public string? ExceptionMessage { get; init; }

    /// <summary>İlişkilendirme (correlation) kimliği.</summary>
    public Guid? CorrelationId { get; init; }

    /// <summary>İsteğin milisaniye cinsinden süresi.</summary>
    public int DurationMs { get; init; }
}
