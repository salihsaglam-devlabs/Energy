using Asp.Versioning;
using Energy.Application.Localization.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Localization.Requests;
using Energy.Shared.Models.V1.Localization.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/localization")]
public sealed class LocalizationController : ControllerBase
{
    private readonly ILocalizationService _localization;
    public LocalizationController(ILocalizationService localization) { _localization = localization; }

    [HttpGet]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<LocalizationEntryResponse>>>> GetAll(CancellationToken ct)
        => Ok(BaseResponse<IReadOnlyList<LocalizationEntryResponse>>.Success(await _localization.GetAllAsync(ct)));

    [HttpGet("{key}")]
    public async Task<ActionResult<BaseResponse<LocalizationEntryResponse>>> GetByKey(string key, CancellationToken ct)
    {
        var item = await _localization.GetByKeyAsync(key, ct);
        return item is null ? NotFound(BaseResponse<LocalizationEntryResponse>.Failure("Key not found."))
                            : Ok(BaseResponse<LocalizationEntryResponse>.Success(item));
    }

    [HttpPost]
    public async Task<ActionResult<BaseResponse<LocalizationEntryResponse>>> Upsert(UpsertLocalizationEntryRequest request, CancellationToken ct)
        => Ok(BaseResponse<LocalizationEntryResponse>.Success(await _localization.UpsertAsync(request, ct)));

    [HttpDelete("{key}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(string key, CancellationToken ct)
        => Ok(BaseResponse<bool>.Success(await _localization.DeleteAsync(key, ct)));
}
