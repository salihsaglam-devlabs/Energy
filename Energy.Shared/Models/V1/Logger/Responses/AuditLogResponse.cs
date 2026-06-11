namespace Energy.Shared.Models.V1.Logger.Responses;

public sealed class AuditLogResponse
{
    public long Id { get; init; }
    public DateTime OccurredAt { get; init; }
    public Guid? UserId { get; init; }
    public string? UserName { get; init; }
    public string? IpAddress { get; init; }
    public string? HttpMethod { get; init; }
    public string? Path { get; init; }
    public string? QueryString { get; init; }
    public int StatusCode { get; init; }
    public bool IsSuccess { get; init; }
    public string? Source { get; init; }
    public string? RequestBody { get; init; }
    public string? ResponseBody { get; init; }
    public bool HasException { get; init; }
    public string? ExceptionType { get; init; }
    public string? ExceptionMessage { get; init; }
    public Guid? CorrelationId { get; init; }
    public int DurationMs { get; init; }
}
