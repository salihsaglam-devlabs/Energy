using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Budget.BudgetLine.Services;
using Energy.Shared.Models.V1.Budget.BudgetLine.Requests;
using Energy.Shared.Models.V1.Budget.BudgetLine.Responses;

namespace Energy.Infrastructure.Budget.BudgetLine.Services;

/// <summary>BudgetLine CRUD servisi (projection, pagination, soft-delete).</summary>
public class BudgetLineService : IBudgetLineService
{
    private readonly AppDbContext _db;

    public BudgetLineService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<BudgetLineListResponse>>> GetListAsync(GetBudgetLineListRequest request, CancellationToken ct = default)
    {
        var query = _db.BudgetLines.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new BudgetLineListResponse
            {
                Id = e.Id,
                BudgetId = e.BudgetId,
                ProjectId = e.ProjectId,
                CostCenterId = e.CostCenterId,
                Description = e.Description,
                PlannedAmount = e.PlannedAmount,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<BudgetLineListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<BudgetLineListResponse>>.Success(page);
    }

    public async Task<BaseResponse<BudgetLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.BudgetLines.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new BudgetLineDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                BudgetId = e.BudgetId,
                ProjectId = e.ProjectId,
                CostCenterId = e.CostCenterId,
                Description = e.Description,
                PlannedAmount = e.PlannedAmount
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<BudgetLineDetailResponse>.Failure("NotFound")
            : BaseResponse<BudgetLineDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateBudgetLineRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Budget.BudgetLine
        {
            Id = Guid.NewGuid(),
            BudgetId = request.BudgetId,
            ProjectId = request.ProjectId,
            CostCenterId = request.CostCenterId,
            Description = request.Description,
            PlannedAmount = request.PlannedAmount,
            CreatedAt = DateTime.UtcNow,
        };
        _db.BudgetLines.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateBudgetLineRequest request, CancellationToken ct = default)
    {
        var entity = await _db.BudgetLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.BudgetId = request.BudgetId;
            entity.ProjectId = request.ProjectId;
            entity.CostCenterId = request.CostCenterId;
            entity.Description = request.Description;
            entity.PlannedAmount = request.PlannedAmount;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.BudgetLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
