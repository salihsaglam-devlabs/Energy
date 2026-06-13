namespace Energy.Web.Configuration;

/// <summary>Web katmanının arka uç API'ye bağlanması için ayarlar.</summary>
public sealed class ApiSettings
{
    /// <summary>Yapılandırma bölümünün adı.</summary>
    public const string SectionName = "Api";

    /// <summary>API'nin temel adresi (base URL).</summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// True olduğunda, HTTP istemcisi API sunucusunun TLS sertifikasını geçersiz olsa
    /// bile (ad uyuşmazlığı / güvenilmeyen zincir / kendinden imzalı) kabul eder.
    /// YALNIZCA sunucu sertifikasının güvenilemediği yerel/geliştirme veya geçici
    /// kurulumlar içindir. Gerçek üretimde false bırakın.
    /// </summary>
    public bool AllowInvalidCertificate { get; set; }
}
