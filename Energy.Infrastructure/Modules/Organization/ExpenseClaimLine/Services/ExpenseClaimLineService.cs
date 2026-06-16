using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Organization.ExpenseClaimLine.Services;
using Energy.Shared.Models.V1.Organization.ExpenseClaimLine.Requests;
using Energy.Shared.Models.V1.Organization.ExpenseClaimLine.Responses;

namespace Energy.Infrastructure.Modules.Organization.ExpenseClaimLine.Services;

/// <summary>ExpenseClaimLine CRUD servisi (projection, pagination, soft-delete).</summary>
public class ExpenseClaimLineService : IExpenseClaimLineService
{
    private readonly AppDbContext _db;

    public ExpenseClaimLineService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ExpenseClaimLineListResponse>>> GetListAsync(GetExpenseClaimLineListRequest request, CancellationToken ct = default)
    {
        var query = _db.ExpenseClaimLines.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ExpenseClaimLineListResponse
            {
                Id = e.Id,
                ExpenseClaimId = e.ExpenseClaimId,
                Description = e.Description,
                ExpenseDate = e.ExpenseDate,
                Amount = e.Amount,
                Category = e.Category,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ExpenseClaimLineListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ExpenseClaimLineListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ExpenseClaimLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ExpenseClaimLines.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ExpenseClaimLineDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                ExpenseClaimId = e.ExpenseClaimId,
                Description = e.Description,
                ExpenseDate = e.ExpenseDate,
                Amount = e.Amount,
                Category = e.Category
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ExpenseClaimLineDetailResponse>.Failure("NotFound")
            : BaseResponse<ExpenseClaimLineDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateExpenseClaimLineRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Organization.ExpenseClaimLine
        {
            Id = Guid.NewGuid(),
            ExpenseClaimId = request.ExpenseClaimId,
            Description = request.Description,
            ExpenseDate = request.ExpenseDate,
            Amount = request.Amount,
            Category = request.Category,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ExpenseClaimLines.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateExpenseClaimLineRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ExpenseClaimLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.ExpenseClaimId = request.ExpenseClaimId;
            entity.Description = request.Description;
            entity.ExpenseDate = request.ExpenseDate;
            entity.Amount = request.Amount;
            entity.Category = request.Category;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ExpenseClaimLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
