using System.Linq;
using Energy.Localization;
using Energy.Shared.Identity.Permissions;
using Energy.Shared.Models.V1.Localization.Requests;
using Energy.Web.Clients.Localization;
using Energy.Web.Common.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Web.Controllers;

/// <summary>
/// Yerelleştirme ızgarası + JSON adaptörü. Yeni API bir (anahtar → kültür → değer)
/// haritası saklar; ızgara bunu düzenleme için kültür başına sütunlara düzleştirir.
/// </summary>
[Authorize]
[PagePermission(PermissionCatalog.LocalizationReadAll)]
[Route("localization")]
public sealed class LocalizationController : Controller
{
    private readonly ILocalizationApiClient _client;

    public LocalizationController(ILocalizationApiClient client) { _client = client; }

    [HttpGet("")]
    [HttpGet("index")]
    public IActionResult Index()
    {
        ViewBag.Cultures = CultureConstants.SupportedCultures.Select(c => c.Name).ToArray();
        return View();
    }

    [HttpGet("list")]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var envelope = await _client.GetAllAsync(ct);
        var items = (envelope.Data ?? Array.Empty<Shared.Models.V1.Localization.Responses.LocalizationEntryResponse>())
            .Select(e =>
            {
                e.Values.TryGetValue("tr-TR", out var tr);
                e.Values.TryGetValue("en-US", out var en);
                e.Values.TryGetValue(string.Empty, out var invariant);
                return new
                {
                    key = e.Key,
                    tr = tr ?? string.Empty,
                    en = en ?? string.Empty,
                    invariant = invariant ?? string.Empty
                };
            })
            .OrderBy(x => x.key, StringComparer.OrdinalIgnoreCase)
            .ToArray();
        return Json(items);
    }

    public sealed class UpsertInput
    {
        public string Key { get; set; } = string.Empty;
        public string? Tr { get; set; }
        public string? En { get; set; }
        public string? Invariant { get; set; }
    }

    [HttpPost("")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Upsert([FromBody] UpsertInput input, CancellationToken ct)
    {
        var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["tr-TR"] = input.Tr ?? string.Empty,
            ["en-US"] = input.En ?? string.Empty,
            [string.Empty] = input.Invariant ?? string.Empty
        };
        var envelope = await _client.UpsertAsync(new UpsertLocalizationEntryRequest
        {
            Key = input.Key,
            Values = values
        }, ct);
        return Json(envelope);
    }

    [HttpDelete("")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Delete([FromQuery] string key, CancellationToken ct)
        => Json(await _client.DeleteAsync(key, ct));

    /// <summary>
    /// Eski "resx'ten içe aktar" araç çubuğu aksiyonu için yer tutucu. Yeni API bir
    /// resx içe aktarma uç noktası sunmaz; bu yüzden arayüzün bir hata göstermemesi için
    /// her zaman sıfır sayaçla başarı yanıtı veririz.
    /// </summary>
    [HttpPost("import-from-resx")]
    [IgnoreAntiforgeryToken]
    public IActionResult ImportFromResx() => Json(new { success = true, data = new { added = 0, updated = 0 } });
}
