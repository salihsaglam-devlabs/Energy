using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.ProgressPayments.ProgressPaymentDeduction.Commands.CreateProgressPaymentDeduction;
using Energy.Application.Modules.ProgressPayments.ProgressPaymentDeduction.Commands.DeleteProgressPaymentDeduction;
using Energy.Application.Modules.ProgressPayments.ProgressPaymentDeduction.Commands.UpdateProgressPaymentDeduction;
using Energy.Application.Modules.ProgressPayments.ProgressPaymentDeduction.Queries.GetProgressPaymentDeductionById;
using Energy.Application.Modules.ProgressPayments.ProgressPaymentDeduction.Queries.GetProgressPaymentDeductionList;
using Energy.Application.Modules.ProgressPayments.ProgressPaymentDeduction.Queries.GetProgressPaymentDeductionLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentDeduction.Requests;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentDeduction.Responses;

namespace Energy.Api.Controllers.ProgressPayments;

/// <summary>
/// ProgressPaymentDeduction uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/progress-payments/progress-payment-deductions")]
public sealed class ProgressPaymentDeductionController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProgressPaymentDeductionController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ProgressPaymentDeductionListResponse>>>> GetList([FromQuery] GetProgressPaymentDeductionListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProgressPaymentDeductionListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ProgressPaymentDeductionDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProgressPaymentDeductionByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ProgressPaymentDeductionLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProgressPaymentDeductionLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateProgressPaymentDeductionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateProgressPaymentDeductionCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateProgressPaymentDeductionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateProgressPaymentDeductionCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteProgressPaymentDeductionCommand(id), ct));
}
