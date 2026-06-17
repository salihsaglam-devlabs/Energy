using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Organization.ExpenseClaimLine.Commands.CreateExpenseClaimLine;
using Energy.Application.Organization.ExpenseClaimLine.Commands.DeleteExpenseClaimLine;
using Energy.Application.Organization.ExpenseClaimLine.Commands.UpdateExpenseClaimLine;
using Energy.Application.Organization.ExpenseClaimLine.Queries.GetExpenseClaimLineById;
using Energy.Application.Organization.ExpenseClaimLine.Queries.GetExpenseClaimLineList;
using Energy.Application.Organization.ExpenseClaimLine.Queries.GetExpenseClaimLineLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.ExpenseClaimLine.Requests;
using Energy.Shared.Models.V1.Organization.ExpenseClaimLine.Responses;

namespace Energy.Api.Controllers.Organization;

/// <summary>
/// ExpenseClaimLine uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/organization/expense-claim-lines")]
public sealed class ExpenseClaimLineController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExpenseClaimLineController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ExpenseClaimLineListResponse>>>> GetList([FromQuery] GetExpenseClaimLineListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetExpenseClaimLineListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ExpenseClaimLineDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetExpenseClaimLineByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ExpenseClaimLineLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetExpenseClaimLineLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateExpenseClaimLineRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateExpenseClaimLineCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateExpenseClaimLineRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateExpenseClaimLineCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteExpenseClaimLineCommand(id), ct));
}
