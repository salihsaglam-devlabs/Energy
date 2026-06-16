using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Core.Branch.Commands.CreateBranch;
using Energy.Application.Modules.Core.Branch.Commands.DeleteBranch;
using Energy.Application.Modules.Core.Branch.Commands.UpdateBranch;
using Energy.Application.Modules.Core.Branch.Queries.GetBranchById;
using Energy.Application.Modules.Core.Branch.Queries.GetBranchList;
using Energy.Application.Modules.Core.Branch.Queries.GetBranchLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Branch.Requests;
using Energy.Shared.Models.V1.Core.Branch.Responses;

namespace Energy.Api.Controllers.Core;

/// <summary>
/// Branch uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/core/branches")]
public sealed class BranchController : ControllerBase
{
    private readonly IMediator _mediator;

    public BranchController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<BranchListResponse>>>> GetList([FromQuery] GetBranchListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetBranchListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<BranchDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetBranchByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<BranchLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetBranchLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateBranchRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateBranchCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateBranchRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateBranchCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteBranchCommand(id), ct));
}
