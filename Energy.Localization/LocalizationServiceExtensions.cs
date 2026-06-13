using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Energy.Localization;

/// <summary>Yerelleştirme altyapısını DI ve istek hattına bağlayan uzantı (extension) metotları.</summary>
public static class LocalizationServiceExtensions
{
    /// <summary>Yerelleştirme servislerini ve istek kültürü (request culture) seçeneklerini kaydeder.</summary>
    public static IServiceCollection AddEnergyLocalization(this IServiceCollection services)
    {
        // SharedResource tipi Energy.Localization ad alanında, .resx dosyaları ise
        // Resources/ alt klasöründe yer alır. ResourcesPath, IStringLocalizer
        // altyapısına "Energy.Localization.Resources.SharedResource.{culture}"
        // konumuna bakmasını söyler — bu da .csproj tarafından üretilen gömülü
        // kaynak manifest adının tam karşılığıdır.
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

    /// <summary>MVC için görünüm (view) ve veri açıklaması (data annotations) yerelleştirmesini ekler.</summary>
    public static IMvcBuilder AddEnergyMvcLocalization(this IMvcBuilder builder)
    {
        return builder
            .AddViewLocalization()
            .AddEnergyDataAnnotationsLocalization();
    }

    /// <summary>Veri açıklaması (data annotations) hatalarını SharedResource üzerinden yerelleştirir.</summary>
    public static IMvcBuilder AddEnergyDataAnnotationsLocalization(this IMvcBuilder builder)
    {
        return builder.AddDataAnnotationsLocalization(options =>
        {
            options.DataAnnotationLocalizerProvider = (_, factory) => factory.Create(typeof(SharedResource));
        });
    }

    /// <summary>İstek kültürü ara katmanını (middleware) yapılandırılmış seçeneklerle hatta ekler.</summary>
    public static IApplicationBuilder UseEnergyRequestLocalization(this IApplicationBuilder app)
    {
        var options = app.ApplicationServices
            .GetRequiredService<IOptions<RequestLocalizationOptions>>()
            .Value;

        app.UseRequestLocalization(options);

        return app;
    }
}
