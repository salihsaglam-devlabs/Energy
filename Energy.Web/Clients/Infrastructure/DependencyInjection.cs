using Energy.Web.Clients.Home;
using Energy.Web.Clients.Identity;
using Energy.Web.Clients.Infrastructure.Authentication;
using Energy.Web.Clients.Infrastructure.ClientIdentity;
using Energy.Web.Clients.Localization;
using Energy.Web.Configuration;
using Energy.Localization;
using Microsoft.Extensions.Options;
// 'System' kelimesi BCL ad alani ile cakistigi icin MenuApiClient'a tam yol ile basvuruyoruz.
using SystemClients = Energy.Web.Clients.System;

namespace Energy.Web.Clients.Infrastructure;

/// <summary>
/// Single entry point that wires every API client and its supporting
/// infrastructure (per-user token reader, client-identity handler, auth
/// handler).
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddEnergyApiClients(this IServiceCollection services)
    {
        services
            .RegisterInfrastructure()
            .RegisterAnonymousClients()
            .RegisterAuthenticatedClients();

        return services;
    }

    private static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<IUserApiTokenProvider, UserApiTokenProvider>();

        services.AddTransient<AuthHeaderHandler>();
        services.AddScoped<BrowserClientIdService>();
        services.AddTransient<ClientIdentityHeaderHandler>();

        return services;
    }

    private static IServiceCollection RegisterAnonymousClients(this IServiceCollection services)
    {
        services.AddAnonymousApiClient<IAuthApiClient, AuthApiClient>();
        return services;
    }

    private static IServiceCollection RegisterAuthenticatedClients(this IServiceCollection services)
    {
        services.AddAuthenticatedApiClient<IHomeApiClient, HomeApiClient>();
        services.AddAuthenticatedApiClient<IUserApiClient, UserApiClient>();
        services.AddAuthenticatedApiClient<IRoleApiClient, RoleApiClient>();
        services.AddAuthenticatedApiClient<IPermissionApiClient, PermissionApiClient>();
        services.AddAuthenticatedApiClient<SystemClients.IMenuApiClient, SystemClients.MenuApiClient>();
        services.AddAuthenticatedApiClient<SystemClients.IAccessRuleApiClient, SystemClients.AccessRuleApiClient>();
        services.AddAuthenticatedApiClient<ILocalizationApiClient, LocalizationApiClient>();
        return services;
    }

    private static void AddAuthenticatedApiClient<TContract, TImplementation>(this IServiceCollection services)
        where TContract : class
        where TImplementation : class, TContract
    {
        services
            .AddHttpClient<TContract, TImplementation>(ConfigureBaseAddress)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
    }

    private static void AddAnonymousApiClient<TContract, TImplementation>(this IServiceCollection services)
        where TContract : class
        where TImplementation : class, TContract
    {
        services
            .AddHttpClient<TContract, TImplementation>(ConfigureBaseAddress)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>();
    }

    private static void ConfigureBaseAddress(IServiceProvider serviceProvider, HttpClient httpClient)
    {
        var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;

        if (string.IsNullOrWhiteSpace(apiSettings.BaseUrl))
        {
            throw new InvalidOperationException(
                LocalizationText.Get(
                    LocalizationKeys.Messages.ApiBaseUrlNotConfigured,
                    "Api:BaseUrl is not configured."));
        }

        httpClient.BaseAddress = new Uri(apiSettings.BaseUrl);
    }
}

