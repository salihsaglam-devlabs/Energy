using Energy.Application.Modules;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Energy.Application;

/// <summary>Energy.Application katmanının DI kayıtları (FluentValidation validator'ları).</summary>
public static class DependencyInjection
{
    /// <summary>
    /// Tüm per-entity FluentValidation validator'larını (Create/Update) ve diğer
    /// application validator'larını derleme tarayarak kaydeder.
    /// </summary>
    public static IServiceCollection AddEnergyApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<ModulesValidatorMarker>(ServiceLifetime.Scoped);
        return services;
    }
}

