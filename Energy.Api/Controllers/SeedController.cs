using Asp.Versioning;
using Energy.Application.Localization.Services;
using Energy.Application.System.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers;

/// <summary>
/// İsteğe bağlı tüm veri tohumlama işlemleri için merkezi yer. Her eylem idempotenttir
/// ve güvenle yeniden çalıştırılabilir. Tam tohumlama, veritabanını kullanılabilir bir
/// temele getirir (şema, yetkiler, roller, kullanıcılar, menüler, uç noktalar,
/// yerelleştirme); ayrıntılı eylemler ise tek bir konuyu yeniden tohumlar.
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
    /// Tüm tohumlama adımlarını çalıştırır (şema tamamlamaları, yetki kataloğu, roller,
    /// demo kullanıcılar, temel menüler, API uç nokta kataloğu ve yerelleştirme). İdempotenttir.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<bool>>> SeedAll(CancellationToken ct)
    {
        await _seeder.SeedAllAsync(ct);
        return Ok(BaseResponse<bool>.Success(true));
    }

    /// <summary>
    /// Veritabanını, uygulamanın gömülü kaynaklarındaki her yerelleştirme girdisiyle
    /// tohumlar. Mevcut (anahtar, kültür) satırları üzerine yazılır; eksik satırlar
    /// eklenir. Diskte kaynak .resx dosyaları olmadan üretimde de çalışır.
    /// </summary>
    [HttpPost("localization")]
    public async Task<ActionResult<BaseResponse<SeedResultResponse>>> SeedLocalization(CancellationToken ct)
        => Ok(BaseResponse<SeedResultResponse>.Success(await _localization.SeedFromResourcesAsync(ct)));

    /// <summary>
    /// Yerelleştirme girdilerini diskteki .resx dosyalarından içe aktarır (geliştirme
    /// kolaylığı; <c>Localization:ResxDirectory</c> ayarlı değilse hiçbir şey yapmaz).
    /// </summary>
    [HttpPost("localization/resx")]
    public async Task<ActionResult<BaseResponse<SeedResultResponse>>> SeedLocalizationFromResx(CancellationToken ct)
        => Ok(BaseResponse<SeedResultResponse>.Success(await _localization.ImportFromResxAsync(ct)));
}

