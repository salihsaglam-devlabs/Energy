using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Core.SystemSetting.Commands.CreateSystemSetting;
using Energy.Application.Modules.Core.SystemSetting.Commands.DeleteSystemSetting;
using Energy.Application.Modules.Core.SystemSetting.Commands.UpdateSystemSetting;
using Energy.Application.Modules.Core.SystemSetting.Queries.GetSystemSettingById;
using Energy.Application.Modules.Core.SystemSetting.Queries.GetSystemSettingList;
using Energy.Application.Modules.Core.SystemSetting.Queries.GetSystemSettingLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.SystemSetting.Requests;
using Energy.Shared.Models.V1.Core.SystemSetting.Responses;

namespace Energy.Api.Controllers.Core;

/// <summary>
/// SystemSetting uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/core/system-settings")]
public sealed class SystemSettingController : ControllerBase
{
    private readonly IMediator _mediator;

    public SystemSettingController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<SystemSettingListResponse>>>> GetList([FromQuery] GetSystemSettingListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetSystemSettingListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<SystemSettingDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetSystemSettingByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<SystemSettingLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetSystemSettingLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateSystemSettingRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateSystemSettingCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateSystemSettingRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateSystemSettingCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteSystemSettingCommand(id), ct));
}
