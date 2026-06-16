using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Organization.LeaveRequest.Commands.CreateLeaveRequest;
using Energy.Application.Modules.Organization.LeaveRequest.Commands.DeleteLeaveRequest;
using Energy.Application.Modules.Organization.LeaveRequest.Commands.UpdateLeaveRequest;
using Energy.Application.Modules.Organization.LeaveRequest.Queries.GetLeaveRequestById;
using Energy.Application.Modules.Organization.LeaveRequest.Queries.GetLeaveRequestList;
using Energy.Application.Modules.Organization.LeaveRequest.Queries.GetLeaveRequestLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.LeaveRequest.Requests;
using Energy.Shared.Models.V1.Organization.LeaveRequest.Responses;

namespace Energy.Api.Controllers.Organization;

/// <summary>
/// LeaveRequest uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/organization/leave-requests")]
public sealed class LeaveRequestController : ControllerBase
{
    private readonly IMediator _mediator;

    public LeaveRequestController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<LeaveRequestListResponse>>>> GetList([FromQuery] GetLeaveRequestListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetLeaveRequestListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<LeaveRequestDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetLeaveRequestByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<LeaveRequestLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetLeaveRequestLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateLeaveRequestRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateLeaveRequestCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateLeaveRequestRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateLeaveRequestCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteLeaveRequestCommand(id), ct));
}
