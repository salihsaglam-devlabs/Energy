using Asp.Versioning;
using Energy.Application.Localization.Services;
using Energy.Application.System.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers;

/// <summary>
/// Central place for all on-demand data seeding operations. Every action is
/// idempotent and safe to re-run. The full seed brings the database to a usable
/// baseline (schema, permissions, roles, users, menus, endpoints, localization);
/// the granular actions re-seed a single concern.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/seed")]
public sealed class SeedController : ControllerBase
{
    private readonly ISystemSeeder _seeder;
    private readonly ILocalizationService _localization;

    public SeedController(ISystemSeeder seeder, ILocalizationService localization)
    {
        _seeder = seeder;
        _localization = localization;
    }

    /// <summary>
    /// Runs every seeding step (schema top-ups, permission catalog, roles, demo
    /// users, baseline menus, API endpoint catalog and localization). Idempotent.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<bool>>> SeedAll(CancellationToken ct)
    {
        await _seeder.SeedAllAsync(ct);
        return Ok(BaseResponse<bool>.Success(true));
    }

    /// <summary>
    /// Seeds the database with every localization entry from the application's
    /// embedded resources. Existing (key, culture) rows are overwritten; missing
    /// rows are inserted. Works in production without source .resx files on disk.
    /// </summary>
    [HttpPost("localization")]
    public async Task<ActionResult<BaseResponse<SeedResultResponse>>> SeedLocalization(CancellationToken ct)
        => Ok(BaseResponse<SeedResultResponse>.Success(await _localization.SeedFromResourcesAsync(ct)));

    /// <summary>
    /// Imports localization entries from the on-disk .resx files (development
    /// convenience; no-op when <c>Localization:ResxDirectory</c> is not set).
    /// </summary>
    [HttpPost("localization/resx")]
    public async Task<ActionResult<BaseResponse<SeedResultResponse>>> SeedLocalizationFromResx(CancellationToken ct)
        => Ok(BaseResponse<SeedResultResponse>.Success(await _localization.ImportFromResxAsync(ct)));
}

