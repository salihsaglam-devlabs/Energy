using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Contracts.ContractLine.Commands.CreateContractLine;
using Energy.Application.Modules.Contracts.ContractLine.Commands.DeleteContractLine;
using Energy.Application.Modules.Contracts.ContractLine.Commands.UpdateContractLine;
using Energy.Application.Modules.Contracts.ContractLine.Queries.GetContractLineById;
using Energy.Application.Modules.Contracts.ContractLine.Queries.GetContractLineList;
using Energy.Application.Modules.Contracts.ContractLine.Queries.GetContractLineLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractLine.Requests;
using Energy.Shared.Models.V1.Contracts.ContractLine.Responses;

namespace Energy.Api.Controllers.Contracts;

/// <summary>
/// ContractLine uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/contracts/contract-lines")]
public sealed class ContractLineController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContractLineController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ContractLineListResponse>>>> GetList([FromQuery] GetContractLineListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetContractLineListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ContractLineDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetContractLineByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ContractLineLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetContractLineLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateContractLineRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateContractLineCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateContractLineRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateContractLineCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteContractLineCommand(id), ct));
}
