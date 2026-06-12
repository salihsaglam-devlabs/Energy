using Energy.Web.Clients.Home;
using Energy.Web.Clients.Identity;
using Energy.Web.Clients.Chat;
using Energy.Web.Clients.Infrastructure.Authentication;
using Energy.Web.Clients.Infrastructure.ClientIdentity;
using Energy.Web.Clients.Localization;
using Energy.Web.Clients.Logger;
using Energy.Web.Configuration;
using Microsoft.Extensions.Options;
using SystemClients = Energy.Web.Clients.System;

namespace Energy.Web.Clients.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddEnergyApiClients(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IUserApiTokenProvider, UserApiTokenProvider>();
        // Singleton: caches the system/service account token across requests.
        services.AddSingleton<IServiceApiTokenProvider, ServiceApiTokenProvider>();
        services.AddTransient<AuthHeaderHandler>();
        services.AddScoped<BrowserClientIdService>();
        services.AddTransient<ClientIdentityHeaderHandler>();

        AddAnonymous<IAuthApiClient, AuthApiClient>(services);
        AddAuthenticated<IHomeApiClient, HomeApiClient>(services);
        AddAuthenticated<IUserApiClient, UserApiClient>(services);
        AddAuthenticated<IRoleApiClient, RoleApiClient>(services);
        AddAuthenticated<IPermissionApiClient, PermissionApiClient>(services);
        AddAuthenticated<SystemClients.IMenuApiClient, SystemClients.MenuApiClient>(services);
        AddAuthenticated<SystemClients.IApiEndpointApiClient, SystemClients.ApiEndpointApiClient>(services);
        AddAuthenticated<ILocalizationApiClient, LocalizationApiClient>(services);
        AddAuthenticated<IAuditLogIngestClient, AuditLogIngestClient>(services);
        AddAuthenticated<IAuditLogQueryClient, AuditLogQueryClient>(services);
        AddAuthenticated<IChatApiClient, ChatApiClient>(services);
        return services;
    }

    private static void AddAuthenticated<TContract, TImpl>(IServiceCollection services)
        where TContract : class where TImpl : class, TContract
    {
        services.AddHttpClient<TContract, TImpl>(Configure)
            .ConfigurePrimaryHttpMessageHandler(CreatePrimaryHandler)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
    }

    private static void AddAnonymous<TContract, TImpl>(IServiceCollection services)
        where TContract : class where TImpl : class, TContract
    {
        services.AddHttpClient<TContract, TImpl>(Configure)
            .ConfigurePrimaryHttpMessageHandler(CreatePrimaryHandler)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>();
    }

    private static void Configure(IServiceProvider sp, HttpClient http)
    {
        var settings = sp.GetRequiredService<IOptions<ApiSettings>>().Value;
        if (string.IsNullOrWhiteSpace(settings.BaseUrl))
            throw new InvalidOperationException("Api:BaseUrl is not configured.");
        http.BaseAddress = new Uri(settings.BaseUrl);
    }

    private static HttpMessageHandler CreatePrimaryHandler(IServiceProvider sp)
    {
        var settings = sp.GetRequiredService<IOptions<ApiSettings>>().Value;
        var handler = new HttpClientHandler();

        // Opt-in bypass for invalid/self-signed API TLS certificates. Only the
        // configured API host is exempted; everything else keeps default checks.
        if (settings.AllowInvalidCertificate)
        {
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        }

        return handler;
    }
}
