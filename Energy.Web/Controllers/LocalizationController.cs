using Energy.Localization;
using Energy.Shared.Models.V1.Localization.Requests;
using Energy.Web.Clients.Localization;
using Energy.Web.Common;
using Energy.Web.Common.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Energy.Web.Controllers;

[Authorize]
[Route("localization")]
[Route("system/localization")]
[ServiceFilter(typeof(ApiExceptionFilter))]
public sealed class LocalizationController : Controller
{
    private readonly ILocalizationApiClient _localizationApiClient;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public LocalizationController(
        ILocalizationApiClient localizationApiClient,
        IStringLocalizer<SharedResource> localizer)
    {
        _localizationApiClient = localizationApiClient;
        _localizer = localizer;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public IActionResult Index()
    {
        ViewData["Title"] = _localizer.GetText(LocalizationKeys.LocalizationScreen.Title);
        return View();
    }

    [HttpGet("list")]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        var envelope = await _localizationApiClient.GetAllAsync(cancellationToken);

        if (!envelope.IsSuccess || envelope.Data is null)
        {
            return BadRequest(new { message = envelope.Message, errors = envelope.Errors });
        }

        // Flatten the per-culture dictionary into a row shape DevExtreme can
        // bind to without nested editors.
        var rows = envelope.Data.Select(entry => new
        {
            key = entry.Key,
            tr = entry.Values.TryGetValue(CultureConstants.TurkishCulture, out var tr) ? tr : null,
            en = entry.Values.TryGetValue(CultureConstants.EnglishCulture, out var en) ? en : null,
            invariant = entry.Values.TryGetValue(string.Empty, out var inv) ? inv : null
        });

        return Ok(rows);
    }

    [HttpPost("")]
    public async Task<IActionResult> Upsert(
        [FromBody] LocalizationRowDto row,
        CancellationToken cancellationToken)
    {
        var values = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(row.Tr)) values[CultureConstants.TurkishCulture] = row.Tr;
        if (!string.IsNullOrEmpty(row.En)) values[CultureConstants.EnglishCulture] = row.En;
        if (!string.IsNullOrEmpty(row.Invariant)) values[string.Empty] = row.Invariant;

        var envelope = await _localizationApiClient.UpsertAsync(
            new UpsertLocalizationEntryRequest
            {
                Key = row.Key,
                Values = values
            },
            cancellationToken);

        return envelope.ToJsonResult();
    }

    [HttpDelete("")]
    public async Task<IActionResult> Delete(
        [FromQuery] string key,
        CancellationToken cancellationToken)
    {
        var envelope = await _localizationApiClient.DeleteAsync(key, cancellationToken);
        return envelope.ToJsonResult();
    }

    [HttpPost("import-from-resx")]
    public async Task<IActionResult> ImportFromResx(CancellationToken cancellationToken)
    {
        var envelope = await _localizationApiClient.ImportFromResxAsync(cancellationToken);
        return envelope.ToJsonResult();
    }

    public sealed class LocalizationRowDto
    {
        public string Key { get; set; } = string.Empty;
        public string? Tr { get; set; }
        public string? En { get; set; }
        public string? Invariant { get; set; }
    }
}

