using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Organization.ExpenseClaim.Commands.CreateExpenseClaim;
using Energy.Application.Modules.Organization.ExpenseClaim.Commands.DeleteExpenseClaim;
using Energy.Application.Modules.Organization.ExpenseClaim.Commands.UpdateExpenseClaim;
using Energy.Application.Modules.Organization.ExpenseClaim.Queries.GetExpenseClaimById;
using Energy.Application.Modules.Organization.ExpenseClaim.Queries.GetExpenseClaimList;
using Energy.Application.Modules.Organization.ExpenseClaim.Queries.GetExpenseClaimLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.ExpenseClaim.Requests;
using Energy.Shared.Models.V1.Organization.ExpenseClaim.Responses;

namespace Energy.Api.Controllers.Organization;

/// <summary>
/// ExpenseClaim uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/organization/expense-claims")]
public sealed class ExpenseClaimController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExpenseClaimController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ExpenseClaimListResponse>>>> GetList([FromQuery] GetExpenseClaimListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetExpenseClaimListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ExpenseClaimDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetExpenseClaimByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ExpenseClaimLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetExpenseClaimLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateExpenseClaimRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateExpenseClaimCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateExpenseClaimRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateExpenseClaimCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteExpenseClaimCommand(id), ct));
}
