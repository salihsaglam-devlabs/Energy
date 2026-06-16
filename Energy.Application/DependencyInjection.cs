using Energy.Application.Common.Messaging.Behaviors;
using Energy.Application.Modules;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Energy.Application;

/// <summary>Energy.Application katmanının DI kayıtları (MediatR + FluentValidation).</summary>
public static class DependencyInjection
{
    /// <summary>
    /// MediatR handler'larını, pipeline behavior'larını ve tüm per-entity FluentValidation
    /// validator'larını (Create/Update + Command/Query) derleme tarayarak kaydeder.
    /// </summary>
    public static IServiceCollection AddEnergyApplication(this IServiceCollection services)
    {
        // MediatR: Energy.Application assembly'sindeki tüm Command/Query handler'ları taranır.
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<ModulesValidatorMarker>();

            // Pipeline behavior'ları (sıra önemlidir): önce logging, sonra validation.
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        // FluentValidation: request-model ve Command/Query validator'ları.
        services.AddValidatorsFromAssemblyContaining<ModulesValidatorMarker>(ServiceLifetime.Scoped);
        return services;
    }
}

