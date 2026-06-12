namespace Energy.Shared.Models.V1.Logger.Requests;

/// <summary>
/// Audit entry posted by an upper layer (e.g. the Web front-end) so that
/// requests handled outside the API are still captured in the single audit
/// sink. Identity (user, IP) and <c>Source</c> are stamped server-side; the
/// caller cannot forge them. Bodies must already be masked but are re-masked on
/// ingest as defense in depth.
/// </summary>
public sealed class CreateAuditLogRequest
{
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Id of the signed-in user the audited request belonged to, or <c>null</c>
    /// for anonymous requests. Only trusted when the entry is forwarded by the
    /// non-interactive system service account (the Web tier always authenticates
    /// audit ingestion as that account), so a human caller can never forge it.
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// User name of the signed-in actor, paired with <see cref="UserId"/>. See
    /// the remarks on <see cref="UserId"/> for the trust model.
    /// </summary>
    public string? UserName { get; set; }

    public string? HttpMethod { get; set; }
    public string? Path { get; set; }
    public string? QueryString { get; set; }
    public int StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public string? RequestBody { get; set; }
    public string? ResponseBody { get; set; }
    public bool HasException { get; set; }
    public string? ExceptionType { get; set; }
    public string? ExceptionMessage { get; set; }
    public Guid? CorrelationId { get; set; }
    public int DurationMs { get; set; }
}

