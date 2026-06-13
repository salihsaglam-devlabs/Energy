namespace Energy.Shared.Models.V1.Logger.Requests;

/// <summary>Denetim (audit) günlüklerini filtrelemek için kullanılan sorgu kriterleri.</summary>
public sealed class AuditLogQueryRequest
{
    /// <summary>Başlangıç zamanı (UTC) — bu andan sonraki kayıtlar.</summary>
    public DateTime? FromUtc { get; set; }

    /// <summary>Bitiş zamanı (UTC) — bu ana kadarki kayıtlar.</summary>
    public DateTime? ToUtc { get; set; }

    /// <summary>Belirli bir kullanıcıya ait kayıtlar.</summary>
    public Guid? UserId { get; set; }

    /// <summary>İstemci IP adresine göre filtre.</summary>
    public string? IpAddress { get; set; }

    /// <summary>HTTP metoduna göre filtre (GET, POST vb.).</summary>
    public string? HttpMethod { get; set; }

    /// <summary>İstek yolunun içerdiği metne göre filtre.</summary>
    public string? PathContains { get; set; }

    /// <summary>HTTP durum koduna göre filtre.</summary>
    public int? StatusCode { get; set; }

    /// <summary>Yalnızca başarılı/başarısız kayıtlara göre filtre.</summary>
    public bool? IsSuccess { get; set; }

    /// <summary>Yalnızca istisna (exception) içeren kayıtlara göre filtre.</summary>
    public bool? HasException { get; set; }

    /// <summary>İlişkilendirme (correlation) kimliğine göre filtre.</summary>
    public Guid? CorrelationId { get; set; }

    /// <summary>Kaydın kaynağına göre filtre (örn. API, Web).</summary>
    public string? Source { get; set; }
}
