using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Contracts.Contract.Commands.CreateContract;
using Energy.Application.Contracts.Contract.Commands.DeleteContract;
using Energy.Application.Contracts.Contract.Commands.UpdateContract;
using Energy.Application.Contracts.Contract.Queries.GetContractById;
using Energy.Application.Contracts.Contract.Queries.GetContractList;
using Energy.Application.Contracts.Contract.Queries.GetContractLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.Contract.Requests;
using Energy.Shared.Models.V1.Contracts.Contract.Responses;

namespace Energy.Api.Controllers.Contracts;

/// <summary>
/// Contract uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/contracts/contracts")]
public sealed class ContractController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContractController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ContractListResponse>>>> GetList([FromQuery] GetContractListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetContractListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ContractDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetContractByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ContractLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetContractLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateContractRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateContractCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateContractRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateContractCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteContractCommand(id), ct));
}
