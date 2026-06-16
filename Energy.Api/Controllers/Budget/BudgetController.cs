using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Budget.Budget.Commands.CreateBudget;
using Energy.Application.Modules.Budget.Budget.Commands.DeleteBudget;
using Energy.Application.Modules.Budget.Budget.Commands.UpdateBudget;
using Energy.Application.Modules.Budget.Budget.Queries.GetBudgetById;
using Energy.Application.Modules.Budget.Budget.Queries.GetBudgetList;
using Energy.Application.Modules.Budget.Budget.Queries.GetBudgetLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Budget.Budget.Requests;
using Energy.Shared.Models.V1.Budget.Budget.Responses;

namespace Energy.Api.Controllers.Budget;

/// <summary>
/// Budget uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/budget/budgets")]
public sealed class BudgetController : ControllerBase
{
    private readonly IMediator _mediator;

    public BudgetController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<BudgetListResponse>>>> GetList([FromQuery] GetBudgetListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetBudgetListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<BudgetDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetBudgetByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<BudgetLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetBudgetLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateBudgetRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateBudgetCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateBudgetRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateBudgetCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteBudgetCommand(id), ct));
}
