namespace Energy.Shared.Models.V1.Core.AuditLog.Requests;

/// <summary>AuditLog oluşturma isteği.</summary>
public class CreateAuditLogRequest
{
    /// <summary>OccurredAt</summary>
    public DateTime OccurredAt { get; set; }

    /// <summary>UserId</summary>
    public Guid? UserId { get; set; }

    /// <summary>UserName</summary>
    public string? UserName { get; set; }

    /// <summary>IpAddress</summary>
    public string? IpAddress { get; set; }

    /// <summary>HttpMethod</summary>
    public string? HttpMethod { get; set; }

    /// <summary>Path</summary>
    public string? Path { get; set; }

    /// <summary>QueryString</summary>
    public string? QueryString { get; set; }

    /// <summary>StatusCode</summary>
    public int StatusCode { get; set; }

    /// <summary>IsSuccess</summary>
    public bool IsSuccess { get; set; }

    /// <summary>Source</summary>
    public string? Source { get; set; }

    /// <summary>RequestBody</summary>
    public string? RequestBody { get; set; }

    /// <summary>ResponseBody</summary>
    public string? ResponseBody { get; set; }

    /// <summary>HasException</summary>
    public bool HasException { get; set; }

    /// <summary>ExceptionType</summary>
    public string? ExceptionType { get; set; }

    /// <summary>ExceptionMessage</summary>
    public string? ExceptionMessage { get; set; }

    /// <summary>CorrelationId</summary>
    public Guid? CorrelationId { get; set; }

    /// <summary>DurationMs</summary>
    public int DurationMs { get; set; }
}
