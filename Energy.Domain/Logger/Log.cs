namespace Energy.Domain.Logger;

/// <summary>
/// Persisted record of a single API request/response, including audit data and
/// any captured exception details. Populated by the request logging middleware.
/// </summary>
public class Log
{
    public Guid Id { get; set; }

    public string TraceId { get; set; } = string.Empty;
    public string CorrelationId { get; set; } = string.Empty;

    public string HttpMethod { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string? QueryString { get; set; }
    public string? RequestHeaders { get; set; }
    public string? RequestPayload { get; set; }
    public string? ContentType { get; set; }

    public int StatusCode { get; set; }
    public string? ResponseHeaders { get; set; }
    public string? ResponsePayload { get; set; }

    public bool IsSuccess { get; set; }
    public long DurationMilliseconds { get; set; }

    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string? UserEmail { get; set; }

    public string? ClientId { get; set; }
    public string? ClientIpAddress { get; set; }
    public string? ClientMachineName { get; set; }
    public string? UserAgent { get; set; }

    public string ServerMachineName { get; set; } = string.Empty;
    public string? ApplicationName { get; set; }
    public string? EnvironmentName { get; set; }

    public bool HasException { get; set; }
    public string? ExceptionType { get; set; }
    public string? ExceptionMessage { get; set; }
    public string? ExceptionStackTrace { get; set; }
    public string? InnerExceptionMessage { get; set; }

    public DateTime RequestStartedAtUtc { get; set; }
    public DateTime RequestCompletedAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
