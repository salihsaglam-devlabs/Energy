using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Finance.Payable.Commands.CreatePayable;
using Energy.Application.Modules.Finance.Payable.Commands.DeletePayable;
using Energy.Application.Modules.Finance.Payable.Commands.UpdatePayable;
using Energy.Application.Modules.Finance.Payable.Queries.GetPayableById;
using Energy.Application.Modules.Finance.Payable.Queries.GetPayableList;
using Energy.Application.Modules.Finance.Payable.Queries.GetPayableLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Payable.Requests;
using Energy.Shared.Models.V1.Finance.Payable.Responses;

namespace Energy.Api.Controllers.Finance;

/// <summary>
/// Payable uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/finance/payables")]
public sealed class PayableController : ControllerBase
{
    private readonly IMediator _mediator;

    public PayableController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<PayableListResponse>>>> GetList([FromQuery] GetPayableListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPayableListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<PayableDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPayableByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<PayableLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPayableLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreatePayableRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreatePayableCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdatePayableRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdatePayableCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeletePayableCommand(id), ct));
}
