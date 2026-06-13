namespace Energy.Shared.Models.V1.Logger.Requests;

/// <summary>
/// Üst bir katman (örn. Web ön yüzü) tarafından gönderilen denetim kaydı; böylece
/// API dışında işlenen istekler de tek denetim havuzunda toplanır. Kimlik (kullanıcı,
/// IP) ve <c>Source</c> sunucu tarafında damgalanır; çağıran bunları sahteleyemez.
/// Gövdeler önceden maskelenmiş olmalıdır ancak savunma derinliği için alımda
/// yeniden maskelenir.
/// </summary>
public sealed class CreateAuditLogRequest
{
    /// <summary>Olayın gerçekleştiği zaman (UTC).</summary>
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Denetlenen isteğin ait olduğu, oturum açmış kullanıcının kimliği; anonim
    /// istekler için <c>null</c>. Yalnızca kayıt etkileşimsiz sistem servis hesabı
    /// tarafından iletildiğinde güvenilirdir (Web katmanı denetim alımını her zaman
    /// o hesapla kimlik doğrular), bu yüzden insan bir çağıran bunu sahteleyemez.
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Oturum açmış aktörün kullanıcı adı; <see cref="UserId"/> ile eşleşir. Güven
    /// modeli için <see cref="UserId"/> açıklamasına bakın.
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>İsteğin HTTP metodu.</summary>
    public string? HttpMethod { get; set; }

    /// <summary>İstek yolu.</summary>
    public string? Path { get; set; }

    /// <summary>İsteğin sorgu dizesi.</summary>
    public string? QueryString { get; set; }

    /// <summary>Yanıtın HTTP durum kodu.</summary>
    public int StatusCode { get; set; }

    /// <summary>İsteğin başarılı olup olmadığı.</summary>
    public bool IsSuccess { get; set; }

    /// <summary>İstek gövdesi (maskelenmiş).</summary>
    public string? RequestBody { get; set; }

    /// <summary>Yanıt gövdesi (maskelenmiş).</summary>
    public string? ResponseBody { get; set; }

    /// <summary>İstek sırasında bir istisna oluşup oluşmadığı.</summary>
    public bool HasException { get; set; }

    /// <summary>İstisnanın tür adı.</summary>
    public string? ExceptionType { get; set; }

    /// <summary>İstisnanın mesajı.</summary>
    public string? ExceptionMessage { get; set; }

    /// <summary>İstekleri uçtan uca izlemek için ilişkilendirme kimliği.</summary>
    public Guid? CorrelationId { get; set; }

    /// <summary>İsteğin milisaniye cinsinden süresi.</summary>
    public int DurationMs { get; set; }
}
