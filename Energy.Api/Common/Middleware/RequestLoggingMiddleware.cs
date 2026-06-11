using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Energy.Api.Common.Logger;
using Energy.Application.Logger.Services;
using Energy.Domain.Logger;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Microsoft.Extensions.Localization;

namespace Energy.Api.Common.Middleware;

public sealed class RequestLoggingMiddleware
{
    private const string CorrelationIdHeaderName = "X-Correlation-Id";
    private const string ClientMachineNameHeaderName = "X-Client-Machine-Name";
    private const string ClientIdHeaderName = "X-Client-Id";

    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public RequestLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestLoggingMiddleware> logger,
        IWebHostEnvironment environment,
        IStringLocalizer<SharedResource> localizer)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
        _localizer = localizer;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ILogService logService)
    {
        var startedAtUtc = DateTime.UtcNow;
        var stopwatch = Stopwatch.StartNew();

        var originalResponseBody = context.Response.Body;

        await using var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        string? requestPayload = null;
        string? responsePayload = null;
        Exception? capturedException = null;

        var correlationId = GetOrCreateCorrelationId(context);

        try
        {
            requestPayload = await ReadRequestPayloadAsync(context.Request);

            await _next(context);
        }
        catch (Exception exception)
        {
            capturedException = exception;

            context.Response.ContentType = "application/json";

            object payload;

            switch (exception)
            {
                case FluentValidation.ValidationException validationException:
                {
                    context.Response.StatusCode =
                        StatusCodes.Status400BadRequest;

                    var errors = validationException.Errors
                        .Select(error =>
                            string.IsNullOrWhiteSpace(error.PropertyName)
                                ? error.ErrorMessage
                                : $"{error.PropertyName}: {error.ErrorMessage}")
                        .Distinct()
                        .ToArray();

                    payload = BaseResponse<object>.Failure(
                        _localizer.GetText(LocalizationKeys.Messages.ValidationFailed, "Validation failed."),
                        errors);

                    break;
                }

                case Energy.Application.Common.Exceptions.NotFoundException:
                {
                    context.Response.StatusCode =
                        StatusCodes.Status404NotFound;

                    payload = BaseResponse<object>.Failure(
                        exception.Message,
                        new[]
                        {
                            exception.Message
                        });

                    break;
                }

                case Energy.Application.Common.Exceptions.ConflictException:
                {
                    context.Response.StatusCode =
                        StatusCodes.Status409Conflict;

                    payload = BaseResponse<object>.Failure(
                        exception.Message,
                        new[]
                        {
                            exception.Message
                        });

                    break;
                }

                default:
                {
                    context.Response.StatusCode =
                        StatusCodes.Status500InternalServerError;

                    payload = BaseResponse<object>.Failure(
                        _localizer.GetText(LocalizationKeys.Messages.UnexpectedError, "An unexpected error occurred."),
                        new[]
                        {
                            exception.Message
                        });

                    break;
                }
            }

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(payload));
        }
        finally
        {
            stopwatch.Stop();

            responseBodyStream.Position = 0;

            responsePayload = await new StreamReader(responseBodyStream)
                .ReadToEndAsync();

            responseBodyStream.Position = 0;

            await responseBodyStream.CopyToAsync(originalResponseBody);

            context.Response.Body = originalResponseBody;

            var log = CreateLog(
                context,
                correlationId,
                requestPayload,
                responsePayload,
                startedAtUtc,
                stopwatch.ElapsedMilliseconds,
                capturedException);

            await SaveLogSafelyAsync(logService, log);
        }
    }

    private Log CreateLog(
        HttpContext context,
        string correlationId,
        string? requestPayload,
        string? responsePayload,
        DateTime startedAtUtc,
        long durationMilliseconds,
        Exception? exception)
    {
        var completedAtUtc = DateTime.UtcNow;

        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? context.User.FindFirstValue("sub");

        var userName = context.User.Identity?.Name
                       ?? context.User.FindFirstValue(ClaimTypes.Name)
                       ?? context.User.FindFirstValue("unique_name");

        var email = context.User.FindFirstValue(ClaimTypes.Email)
                    ?? context.User.FindFirstValue("email");

        var ipAddress = context.Connection.RemoteIpAddress?.ToString();

        var clientMachineName =
            context.Request.Headers[ClientMachineNameHeaderName]
                .FirstOrDefault();

        var clientId =
            context.Request.Headers[ClientIdHeaderName]
                .FirstOrDefault();
        
        var statusCode = context.Response.StatusCode;

        return new Log
        {
            Id = Guid.NewGuid(),

            TraceId = context.TraceIdentifier,
            CorrelationId = correlationId,

            HttpMethod = context.Request.Method,
            Path = context.Request.Path.Value ?? string.Empty,
            QueryString = context.Request.QueryString.HasValue
                ? context.Request.QueryString.Value
                : null,

            RequestHeaders = SerializeHeaders(
                context.Request.Headers,
                excludeAuthorizationHeader: true),

            RequestPayload = SensitiveDataMasker.MaskJson(requestPayload),

            ContentType = context.Request.ContentType,

            StatusCode = statusCode,

            ResponseHeaders = SerializeHeaders(
                context.Response.Headers,
                excludeAuthorizationHeader: false),

            ResponsePayload = SensitiveDataMasker.MaskJson(responsePayload),

            IsSuccess = exception is null
                        && statusCode >= StatusCodes.Status200OK
                        && statusCode < StatusCodes.Status400BadRequest,

            DurationMilliseconds = durationMilliseconds,

            ClientId = clientId,
            UserId = userId,
            UserName = userName,
            UserEmail = email,

            ClientIpAddress = ipAddress,
            ClientMachineName = clientMachineName,
            UserAgent = context.Request.Headers.UserAgent.FirstOrDefault(),

            ServerMachineName = Environment.MachineName,
            ApplicationName = _environment.ApplicationName,
            EnvironmentName = _environment.EnvironmentName,

            HasException = exception is not null,
            ExceptionType = exception?.GetType().FullName,
            ExceptionMessage = exception?.Message,
            ExceptionStackTrace = exception?.StackTrace,
            InnerExceptionMessage = exception?.InnerException?.Message,

            RequestStartedAtUtc = startedAtUtc,
            RequestCompletedAtUtc = completedAtUtc,
            CreatedAtUtc = completedAtUtc
        };
    }

    private async Task SaveLogSafelyAsync(
        ILogService apiRequestLogService,
        Log log)
    {
        try
        {
            await apiRequestLogService.CreateAsync(log);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "API request log could not be written to database. CorrelationId: {CorrelationId}",
                log.CorrelationId);
        }
    }

    private async Task<string?> ReadRequestPayloadAsync(
        HttpRequest request)
    {
        if (!request.Body.CanRead)
        {
            return null;
        }

        if (!IsTextBasedContentType(request.ContentType))
        {
            return string.Format(
                _localizer.GetText(LocalizationKeys.Messages.PayloadLoggingSkipped, "Payload logging skipped. Content-Type: {0}"),
                request.ContentType);
        }

        request.EnableBuffering();

        request.Body.Position = 0;

        using var reader = new StreamReader(
            request.Body,
            Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false,
            leaveOpen: true);

        var payload = await reader.ReadToEndAsync();

        request.Body.Position = 0;

        return string.IsNullOrWhiteSpace(payload)
            ? null
            : payload;
    }

    private static string GetOrCreateCorrelationId(HttpContext context)
    {
        var correlationId =
            context.Request.Headers[CorrelationIdHeaderName]
                .FirstOrDefault();

        if (string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = Guid.NewGuid().ToString("N");
        }

        context.Response.Headers[CorrelationIdHeaderName] = correlationId;

        return correlationId;
    }

    private static string SerializeHeaders(
        IHeaderDictionary headers,
        bool excludeAuthorizationHeader)
    {
        var dictionary = headers
            .Where(header =>
                !excludeAuthorizationHeader
                || !string.Equals(
                    header.Key,
                    "Authorization",
                    StringComparison.OrdinalIgnoreCase))
            .ToDictionary(
                header => header.Key,
                header => header.Value.ToString());

        return JsonSerializer.Serialize(dictionary);
    }

    private static bool IsTextBasedContentType(string? contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType))
        {
            return true;
        }

        return contentType.Contains(
                   "application/json",
                   StringComparison.OrdinalIgnoreCase)
               || contentType.Contains(
                   "application/xml",
                   StringComparison.OrdinalIgnoreCase)
               || contentType.Contains(
                   "application/x-www-form-urlencoded",
                   StringComparison.OrdinalIgnoreCase)
               || contentType.Contains(
                   "text/",
                   StringComparison.OrdinalIgnoreCase);
    }
}