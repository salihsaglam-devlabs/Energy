using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Contracts.ContractAmendment.Commands.CreateContractAmendment;
using Energy.Application.Modules.Contracts.ContractAmendment.Commands.DeleteContractAmendment;
using Energy.Application.Modules.Contracts.ContractAmendment.Commands.UpdateContractAmendment;
using Energy.Application.Modules.Contracts.ContractAmendment.Queries.GetContractAmendmentById;
using Energy.Application.Modules.Contracts.ContractAmendment.Queries.GetContractAmendmentList;
using Energy.Application.Modules.Contracts.ContractAmendment.Queries.GetContractAmendmentLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractAmendment.Requests;
using Energy.Shared.Models.V1.Contracts.ContractAmendment.Responses;

namespace Energy.Api.Controllers.Contracts;

/// <summary>
/// ContractAmendment uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/contracts/contract-amendments")]
public sealed class ContractAmendmentController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContractAmendmentController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ContractAmendmentListResponse>>>> GetList([FromQuery] GetContractAmendmentListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetContractAmendmentListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ContractAmendmentDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetContractAmendmentByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ContractAmendmentLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetContractAmendmentLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateContractAmendmentRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateContractAmendmentCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateContractAmendmentRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateContractAmendmentCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteContractAmendmentCommand(id), ct));
}
