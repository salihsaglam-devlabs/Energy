using Energy.Shared.Identity;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Web.Clients.Identity;

namespace Energy.Web.Clients.Infrastructure.Authentication;

/// <summary>
/// Etkileşimsiz sistem/servis hesabı için bir bearer token alır ve önbelleğe alır.
/// Oturum açmış herhangi bir kullanıcıdan bağımsız olarak API'yi çağırması gereken
/// dahili/sistem akışları (ör. anonim isteklerin denetlenmesi) tarafından kullanılır.
/// </summary>
public interface IServiceApiTokenProvider
{
    /// <summary>Geçerli bir servis erişim jetonunu döndürür (gerekirse yeniler).</summary>
    Task<string?> GetAccessTokenAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Yapılandırılan servis hesabıyla API'ye giriş yapan ve JWT'yi süresi dolmadan kısa
/// bir süre öncesine kadar önbellekte tutan, istendiğinde yenileyen singleton sağlayıcı.
/// Günlükleme isteği asla bozmamalıdır; bu yüzden hatalar istisna fırlatmak yerine
/// <c>null</c> döndürür.
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

    /// <summary>Kapsam fabrikasını, yapılandırmayı ve günlükleyiciyi enjekte eder.</summary>
    public ServiceApiTokenProvider(
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration,
        ILogger<ServiceApiTokenProvider> logger)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>Önbellekteki jetonu döndürür veya süresi dolmuşsa yeniden alır.</summary>
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

            // Auth istemcisi scoped, anonim tipli bir istemci olarak kayıtlıdır; bu
            // sağlayıcı singleton olduğundan onu kısa ömürlü bir kapsam içinde çöz.
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

    /// <summary>Önbellekteki jetonun hâlâ kullanılabilir (süresi yakında dolmayan) olup olmadığını belirler.</summary>
    private bool IsTokenUsable()
        => !string.IsNullOrEmpty(_token) && DateTime.UtcNow < _expiresAtUtc - ExpiryMargin;
}

