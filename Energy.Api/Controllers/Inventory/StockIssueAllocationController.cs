using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Inventory.StockIssueAllocation.Commands.CreateStockIssueAllocation;
using Energy.Application.Modules.Inventory.StockIssueAllocation.Commands.DeleteStockIssueAllocation;
using Energy.Application.Modules.Inventory.StockIssueAllocation.Commands.UpdateStockIssueAllocation;
using Energy.Application.Modules.Inventory.StockIssueAllocation.Queries.GetStockIssueAllocationById;
using Energy.Application.Modules.Inventory.StockIssueAllocation.Queries.GetStockIssueAllocationList;
using Energy.Application.Modules.Inventory.StockIssueAllocation.Queries.GetStockIssueAllocationLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockIssueAllocation.Requests;
using Energy.Shared.Models.V1.Inventory.StockIssueAllocation.Responses;

namespace Energy.Api.Controllers.Inventory;

/// <summary>
/// StockIssueAllocation uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/inventory/stock-issue-allocations")]
public sealed class StockIssueAllocationController : ControllerBase
{
    private readonly IMediator _mediator;

    public StockIssueAllocationController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<StockIssueAllocationListResponse>>>> GetList([FromQuery] GetStockIssueAllocationListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockIssueAllocationListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<StockIssueAllocationDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockIssueAllocationByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<StockIssueAllocationLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetStockIssueAllocationLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateStockIssueAllocationRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateStockIssueAllocationCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateStockIssueAllocationRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateStockIssueAllocationCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteStockIssueAllocationCommand(id), ct));
}
