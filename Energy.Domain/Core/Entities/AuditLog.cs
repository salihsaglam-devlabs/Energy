namespace Energy.Domain.Core;

/// <summary>
/// Yalnızca-ekleme (append-only) istek denetim (audit) kaydı. Sadece INSERT
/// yapılır; UPDATE/DELETE işlemleri veritabanı rolü seviyesinde iptal edilmelidir.
/// </summary>
public class AuditLog
{
    /// <summary>Otomatik artan birincil anahtar.</summary>
    public long Id { get; set; }

    /// <summary>İsteğin gerçekleştiği UTC zaman damgası.</summary>
    public DateTime OccurredAt { get; set; }

    /// <summary>İsteği yapan kullanıcının kimliği (anonimse null).</summary>
    public Guid? UserId { get; set; }

    /// <summary>İsteği yapan kullanıcının adı (varsa).</summary>
    public string? UserName { get; set; }

    /// <summary>İsteğin geldiği IP adresi.</summary>
    public string? IpAddress { get; set; }

    /// <summary>HTTP metodu (GET, POST, ...).</summary>
    public string? HttpMethod { get; set; }

    /// <summary>İstek yolu (path).</summary>
    public string? Path { get; set; }

    /// <summary>Sorgu dizesi (query string).</summary>
    public string? QueryString { get; set; }

    /// <summary>Yanıtın HTTP durum kodu.</summary>
    public int StatusCode { get; set; }

    /// <summary>İsteğin başarılı olup olmadığı.</summary>
    public bool IsSuccess { get; set; }

    /// <summary>Kaydı üreten katman: "API" veya "Web".</summary>
    public string? Source { get; set; }

    /// <summary>Maskelenmiş istek gövdesi (hassas alanlar gizlenmiş).</summary>
    public string? RequestBody { get; set; }

    /// <summary>Maskelenmiş yanıt gövdesi (hassas alanlar gizlenmiş).</summary>
    public string? ResponseBody { get; set; }

    /// <summary>İstek sırasında bir istisna oluşup oluşmadığı.</summary>
    public bool HasException { get; set; }

    /// <summary>Oluşan istisnanın tip adı (varsa).</summary>
    public string? ExceptionType { get; set; }

    /// <summary>İstisnanın mesajı (varsa).</summary>
    public string? ExceptionMessage { get; set; }

    /// <summary>İlişkilendirme (correlation) kimliği; katmanlar arası izleme için.</summary>
    public Guid? CorrelationId { get; set; }

    /// <summary>İsteğin süresi (milisaniye).</summary>
    public int DurationMs { get; set; }
}
