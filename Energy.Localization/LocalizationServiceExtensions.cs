using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Energy.Localization;

public static class LocalizationServiceExtensions
{
    public static IServiceCollection AddEnergyLocalization(this IServiceCollection services)
    {
        // SharedResource type lives in the Energy.Localization namespace while
        // the .resx files live in the Resources/ subfolder. ResourcesPath tells
        // the IStringLocalizer infrastructure to look at
        // "Energy.Localization.Resources.SharedResource.{culture}" — which is
        // exactly the embedded resource manifest name produced by the .csproj.
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture(CultureConstants.DefaultCulture);
            options.SupportedCultures = CultureConstants.SupportedCultures;
            options.SupportedUICultures = CultureConstants.SupportedCultures;

            options.RequestCultureProviders =
            [
                new QueryStringRequestCultureProvider(),
                new CookieRequestCultureProvider(),
                new AcceptLanguageHeaderRequestCultureProvider()
            ];
        });

        return services;
    }

    public static IMvcBuilder AddEnergyMvcLocalization(this IMvcBuilder builder)
    {
        return builder
            .AddViewLocalization()
            .AddEnergyDataAnnotationsLocalization();
    }

    public static IMvcBuilder AddEnergyDataAnnotationsLocalization(this IMvcBuilder builder)
    {
        return builder.AddDataAnnotationsLocalization(options =>
        {
            options.DataAnnotationLocalizerProvider = (_, factory) => factory.Create(typeof(SharedResource));
        });
    }

    public static IApplicationBuilder UseEnergyRequestLocalization(this IApplicationBuilder app)
    {
        var options = app.ApplicationServices
            .GetRequiredService<IOptions<RequestLocalizationOptions>>()
            .Value;

        app.UseRequestLocalization(options);

        return app;
    }
}
