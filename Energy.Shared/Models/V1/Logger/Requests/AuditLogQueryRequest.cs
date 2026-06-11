namespace Energy.Shared.Models.V1.Logger.Requests;

public sealed class AuditLogQueryRequest
{
    public DateTime? FromUtc { get; set; }
    public DateTime? ToUtc { get; set; }
    public Guid? UserId { get; set; }
    public string? IpAddress { get; set; }
    public string? HttpMethod { get; set; }
    public string? PathContains { get; set; }
    public int? StatusCode { get; set; }
    public bool? IsSuccess { get; set; }
    public bool? HasException { get; set; }
    public Guid? CorrelationId { get; set; }
    public string? Source { get; set; }
}
