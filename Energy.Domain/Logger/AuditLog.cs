namespace Energy.Domain.Logger;

/// <summary>
/// Append-only request audit record. Inserts only; UPDATE/DELETE must be
/// revoked at the database role level.
/// </summary>
public class AuditLog
{
    public long Id { get; set; }

    public DateTime OccurredAt { get; set; }
    public Guid? UserId { get; set; }
    public string? UserName { get; set; }
    public string? IpAddress { get; set; }

    public string? HttpMethod { get; set; }
    public string? Path { get; set; }
    public string? QueryString { get; set; }
    public int StatusCode { get; set; }
    public bool IsSuccess { get; set; }

    /// <summary>Originating layer: "API" or "Web".</summary>
    public string? Source { get; set; }

    /// <summary>Masked request payload (sensitive fields redacted).</summary>
    public string? RequestBody { get; set; }

    /// <summary>Masked response payload (sensitive fields redacted).</summary>
    public string? ResponseBody { get; set; }

    public bool HasException { get; set; }
    public string? ExceptionType { get; set; }
    public string? ExceptionMessage { get; set; }

    public Guid? CorrelationId { get; set; }
    public int DurationMs { get; set; }
}

