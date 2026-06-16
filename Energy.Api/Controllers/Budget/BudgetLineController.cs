using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Budget.BudgetLine.Commands.CreateBudgetLine;
using Energy.Application.Budget.BudgetLine.Commands.DeleteBudgetLine;
using Energy.Application.Budget.BudgetLine.Commands.UpdateBudgetLine;
using Energy.Application.Budget.BudgetLine.Queries.GetBudgetLineById;
using Energy.Application.Budget.BudgetLine.Queries.GetBudgetLineList;
using Energy.Application.Budget.BudgetLine.Queries.GetBudgetLineLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Budget.BudgetLine.Requests;
using Energy.Shared.Models.V1.Budget.BudgetLine.Responses;

namespace Energy.Api.Controllers.Budget;

/// <summary>
/// BudgetLine uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/budget/budget-lines")]
public sealed class BudgetLineController : ControllerBase
{
    private readonly IMediator _mediator;

    public BudgetLineController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<BudgetLineListResponse>>>> GetList([FromQuery] GetBudgetLineListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetBudgetLineListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<BudgetLineDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetBudgetLineByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<BudgetLineLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetBudgetLineLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateBudgetLineRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateBudgetLineCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateBudgetLineRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateBudgetLineCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteBudgetLineCommand(id), ct));
}
