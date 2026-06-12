using Energy.Shared.Identity;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Web.Clients.Identity;

namespace Energy.Web.Clients.Infrastructure.Authentication;

/// <summary>
/// Obtains and caches a bearer token for the non-interactive system/service
/// account. Used by internal/system flows (e.g. auditing anonymous requests)
/// that must call the API independently of any signed-in user.
/// </summary>
public interface IServiceApiTokenProvider
{
    Task<string?> GetAccessTokenAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Singleton provider that logs the configured service account into the API and
/// caches the JWT until shortly before it expires, refreshing on demand. Logging
/// must never break the request, so failures return <c>null</c> instead of
/// throwing.
/// </summary>
public sealed class ServiceApiTokenProvider : IServiceApiTokenProvider
{
    private static readonly TimeSpan ExpiryMargin = TimeSpan.FromMinutes(1);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ServiceApiTokenProvider> _logger;
    private readonly SemaphoreSlim _gate = new(1, 1);

    private string? _token;
    private DateTime _expiresAtUtc;

    public ServiceApiTokenProvider(
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration,
        ILogger<ServiceApiTokenProvider> logger)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string?> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        if (IsTokenUsable()) return _token;

        await _gate.WaitAsync(cancellationToken);
        try
        {
            if (IsTokenUsable()) return _token;

            var userName = _configuration[ServiceAccount.WebUserNameConfigKey];
            if (string.IsNullOrWhiteSpace(userName)) userName = ServiceAccount.Email;
            var password = _configuration[ServiceAccount.WebPasswordConfigKey];
            if (string.IsNullOrWhiteSpace(password)) password = ServiceAccount.DefaultPassword;

            // The auth client is registered as a scoped, anonymous typed client;
            // resolve it inside a short-lived scope since this provider is a singleton.
            using var scope = _scopeFactory.CreateScope();
            var auth = scope.ServiceProvider.GetRequiredService<IAuthApiClient>();

            var response = await auth.LoginAsync(
                new LoginRequest { UserNameOrEmail = userName, Password = password },
                cancellationToken);

            if (!response.IsSuccess || response.Data is null)
            {
                _logger.LogError("[ServiceToken] Service account login failed: {Message}", response.Message);
                _token = null;
                return null;
            }

            _token = response.Data.AccessToken;
            _expiresAtUtc = response.Data.ExpiresAt;
            _logger.LogInformation("[ServiceToken] Acquired service token (expires {ExpiresAt:o}).", _expiresAtUtc);
            return _token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ServiceToken] Could not obtain a service access token.");
            _token = null;
            return null;
        }
        finally
        {
            _gate.Release();
        }
    }

    private bool IsTokenUsable()
        => !string.IsNullOrEmpty(_token) && DateTime.UtcNow < _expiresAtUtc - ExpiryMargin;
}

