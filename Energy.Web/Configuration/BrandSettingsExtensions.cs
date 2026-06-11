using Microsoft.AspNetCore.Mvc;

namespace Energy.Web.Configuration;

public static class BrandSettingsExtensions
{
    public static string? GetLogoSource(this BrandSettings? brand, IUrlHelper urlHelper)
    {
        if (brand is null)
        {
            return null;
        }

        if (!string.IsNullOrWhiteSpace(brand.LogoUrl))
        {
            return brand.LogoUrl;
        }

        if (!string.IsNullOrWhiteSpace(brand.LogoPath))
        {
            return urlHelper.Content(brand.LogoPath);
        }

        return null;
    }
}

