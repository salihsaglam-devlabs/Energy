using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.FieldOperations.MeasurementSheetLine.Services;
using Energy.Application.Modules.FieldOperations.MeasurementSheetLine.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheetLine.Requests;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheetLine.Responses;

namespace Energy.Api.Controllers.Modules.FieldOperations;

/// <summary>MeasurementSheetLine uç noktaları (liste, detay, lookup, create, update, delete).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/field-operations/measurement-sheet-lines")]
public sealed class MeasurementSheetLineController : ControllerBase
{
    private readonly IMeasurementSheetLineService _service;
    private readonly IMeasurementSheetLineLookupService _lookup;

    public MeasurementSheetLineController(IMeasurementSheetLineService service, IMeasurementSheetLineLookupService lookup)
    {
        _service = service;
        _lookup = lookup;
    }

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<MeasurementSheetLineListResponse>>>> GetList([FromQuery] GetMeasurementSheetLineListRequest request, CancellationToken ct)
        => Ok(await _service.GetListAsync(request, ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<MeasurementSheetLineDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _service.GetByIdAsync(id, ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<MeasurementSheetLineLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _lookup.GetLookupAsync(search, activeOnly, ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateMeasurementSheetLineRequest request, CancellationToken ct)
        => Ok(await _service.CreateAsync(request, ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateMeasurementSheetLineRequest request, CancellationToken ct)
        => Ok(await _service.UpdateAsync(id, request, ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _service.DeleteAsync(id, ct));
}
