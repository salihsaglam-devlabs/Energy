using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Budget.Budget.Services;
using Energy.Shared.Models.V1.Budget.Budget.Requests;
using Energy.Shared.Models.V1.Budget.Budget.Responses;

namespace Energy.Infrastructure.Modules.Budget.Budget.Services;

/// <summary>Budget CRUD servisi (projection, pagination, soft-delete).</summary>
public class BudgetService : IBudgetService
{
    private readonly AppDbContext _db;

    public BudgetService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<BudgetListResponse>>> GetListAsync(GetBudgetListRequest request, CancellationToken ct = default)
    {
        var query = _db.Budgets.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new BudgetListResponse
            {
                Id = e.Id,
                ProjectId = e.ProjectId,
                CostCenterId = e.CostCenterId,
                CurrencyId = e.CurrencyId,
                Name = e.Name,
                Year = e.Year,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<BudgetListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<BudgetListResponse>>.Success(page);
    }

    public async Task<BaseResponse<BudgetDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.Budgets.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new BudgetDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                ProjectId = e.ProjectId,
                CostCenterId = e.CostCenterId,
                CurrencyId = e.CurrencyId,
                Name = e.Name,
                Year = e.Year,
                IsActive = e.IsActive
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<BudgetDetailResponse>.Failure("NotFound")
            : BaseResponse<BudgetDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateBudgetRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Budget.Budget
        {
            Id = Guid.NewGuid(),
            ProjectId = request.ProjectId,
            CostCenterId = request.CostCenterId,
            CurrencyId = request.CurrencyId,
            Name = request.Name,
            Year = request.Year,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
        };
        _db.Budgets.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateBudgetRequest request, CancellationToken ct = default)
    {
        var entity = await _db.Budgets.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.ProjectId = request.ProjectId;
            entity.CostCenterId = request.CostCenterId;
            entity.CurrencyId = request.CurrencyId;
            entity.Name = request.Name;
            entity.Year = request.Year;
            entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.Budgets.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
