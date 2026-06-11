using Asp.Versioning;
using Energy.Application.Localization.Commands.DeleteLocalizationEntry;
using Energy.Application.Localization.Commands.ImportLocalizationFromResx;
using Energy.Application.Localization.Commands.UpsertLocalizationEntry;
using Energy.Application.Localization.Queries.GetLocalizationEntries;
using Energy.Application.Localization.Queries.GetLocalizationEntryByKey;
using Energy.Localization;
using Energy.Shared.Identity.Permissions;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Localization.Requests;
using Energy.Shared.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Energy.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/localization")]
[Authorize]
public sealed class LocalizationController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public LocalizationController(ISender sender, IStringLocalizer<SharedResource> localizer)
    {
        _sender = sender;
        _localizer = localizer;
    }

    /// <summary>
    /// Lists every (key → values-by-culture) override currently stored in the database.
    /// </summary>
    [HttpGet]
    [Authorize(Policy = LocalizationPermissions.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new GetLocalizationEntriesQuery(), cancellationToken);
        return Ok(response);
    }

    [HttpGet("{key}")]
    [Authorize(Policy = LocalizationPermissions.GetByKey)]
    public async Task<IActionResult> GetByKey(string key, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new GetLocalizationEntryByKeyQuery(key), cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Inserts or updates the supplied (culture → value) pairs for the given
    /// key. Persists to the database and (when the resx writer is enabled)
    /// also updates the on-disk .resx files.
    /// </summary>
    [HttpPost]
    [Authorize(Policy = LocalizationPermissions.Upsert)]
    public async Task<IActionResult> Upsert(
        [FromBody] UpsertLocalizationEntryRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new UpsertLocalizationEntryCommand(request), cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{key}")]
    [Authorize(Policy = LocalizationPermissions.Delete)]
    public async Task<IActionResult> Delete(string key, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new DeleteLocalizationEntryCommand(key), cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// One-shot bootstrap: reads every (culture, key, value) from the on-disk
    /// .resx files and inserts/updates them in the database so the DB-first
    /// localizer can serve them.
    /// </summary>
    [HttpPost("import-from-resx")]
    [AllowAnonymous]
    public async Task<IActionResult> ImportFromResx(CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new ImportLocalizationFromResxCommand(), cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Diagnostic endpoint that resolves a handful of keys via the active
    /// <see cref="IStringLocalizer{SharedResource}"/>, which now consults the
    /// database before falling back to .resx.
    /// </summary>
    [HttpGet("test")]
    [AllowAnonymous]
    public IActionResult Test()
    {
        var data = new
        {
            CurrentCulture = Thread.CurrentThread.CurrentCulture.Name,
            CurrentUICulture = Thread.CurrentThread.CurrentUICulture.Name,
            Login = _localizer.GetText(LocalizationKeys.Common.Login),
            Save = _localizer.GetText(LocalizationKeys.Common.Save),
            Cancel = _localizer.GetText(LocalizationKeys.Common.Cancel),
            RoleDisplayName = _localizer.GetText(LocalizationKeys.Roles.AdminDisplayName)
        };

        return Ok(BaseResponse<object>.Success(data));
    }
}
