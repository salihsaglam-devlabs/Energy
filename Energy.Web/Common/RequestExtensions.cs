namespace Energy.Web.Common;

/// <summary>
/// AJAX/JSON (XHR/fetch) isteklerini tam sayfa tarayıcı gezinmelerinden ayırt eden
/// yardımcılar. Kimlik doğrulama filtreleri, makine tarafından okunabilir bir JSON
/// zarfı mı (istemcinin kendini yönlendirebilmesi için) yoksa üst seviye gezinmeler
/// için klasik bir 302 yönlendirmesi mi yayınlayacağına karar vermek için bunu kullanır.
/// </summary>
public static class RequestExtensions
{
    /// <summary>
    /// İstek, uygulama içi HTTP yardımcısı / DevExtreme tarafından yapıldığında true olur
    /// (bunlar <c>X-Requested-With: XMLHttpRequest</c> ve/veya
    /// <c>Accept: application/json</c> ayarlar). Bunlar için HTML yönlendirmesi
    /// döndürmek işe yaramaz; çünkü <c>fetch</c> bunları şeffaf şekilde izler ve ızgara
    /// (grid) ardından bir HTML sayfasını JSON olarak ayrıştırmaya çalışır.
    /// </summary>
    public static bool WantsJson(this HttpRequest request)
    {
        if (string.Equals(request.Headers.XRequestedWith, "XMLHttpRequest", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        var accept = request.Headers.Accept.ToString();
        return accept.Contains("application/json", StringComparison.OrdinalIgnoreCase);
    }
}

