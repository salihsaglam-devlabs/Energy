using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Contracts.ContractParty.Commands.CreateContractParty;
using Energy.Application.Modules.Contracts.ContractParty.Commands.DeleteContractParty;
using Energy.Application.Modules.Contracts.ContractParty.Commands.UpdateContractParty;
using Energy.Application.Modules.Contracts.ContractParty.Queries.GetContractPartyById;
using Energy.Application.Modules.Contracts.ContractParty.Queries.GetContractPartyList;
using Energy.Application.Modules.Contracts.ContractParty.Queries.GetContractPartyLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractParty.Requests;
using Energy.Shared.Models.V1.Contracts.ContractParty.Responses;

namespace Energy.Api.Controllers.Contracts;

/// <summary>
/// ContractParty uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/contracts/contract-parties")]
public sealed class ContractPartyController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContractPartyController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ContractPartyListResponse>>>> GetList([FromQuery] GetContractPartyListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetContractPartyListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ContractPartyDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetContractPartyByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ContractPartyLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetContractPartyLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateContractPartyRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateContractPartyCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateContractPartyRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateContractPartyCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteContractPartyCommand(id), ct));
}
