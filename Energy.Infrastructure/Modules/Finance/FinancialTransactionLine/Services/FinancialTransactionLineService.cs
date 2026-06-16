using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Finance.FinancialTransactionLine.Services;
using Energy.Shared.Models.V1.Finance.FinancialTransactionLine.Requests;
using Energy.Shared.Models.V1.Finance.FinancialTransactionLine.Responses;

namespace Energy.Infrastructure.Modules.Finance.FinancialTransactionLine.Services;

/// <summary>FinancialTransactionLine CRUD servisi (projection, pagination, soft-delete).</summary>
public class FinancialTransactionLineService : IFinancialTransactionLineService
{
    private readonly AppDbContext _db;

    public FinancialTransactionLineService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<FinancialTransactionLineListResponse>>> GetListAsync(GetFinancialTransactionLineListRequest request, CancellationToken ct = default)
    {
        var query = _db.FinancialTransactionLines.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new FinancialTransactionLineListResponse
            {
                Id = e.Id,
                FinancialTransactionId = e.FinancialTransactionId,
                CostCenterId = e.CostCenterId,
                ProjectId = e.ProjectId,
                Amount = e.Amount,
                Description = e.Description,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<FinancialTransactionLineListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<FinancialTransactionLineListResponse>>.Success(page);
    }

    public async Task<BaseResponse<FinancialTransactionLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.FinancialTransactionLines.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new FinancialTransactionLineDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                FinancialTransactionId = e.FinancialTransactionId,
                CostCenterId = e.CostCenterId,
                ProjectId = e.ProjectId,
                Amount = e.Amount,
                Description = e.Description
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<FinancialTransactionLineDetailResponse>.Failure("NotFound")
            : BaseResponse<FinancialTransactionLineDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateFinancialTransactionLineRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Finance.FinancialTransactionLine
        {
            Id = Guid.NewGuid(),
            FinancialTransactionId = request.FinancialTransactionId,
            CostCenterId = request.CostCenterId,
            ProjectId = request.ProjectId,
            Amount = request.Amount,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow,
        };
        _db.FinancialTransactionLines.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateFinancialTransactionLineRequest request, CancellationToken ct = default)
    {
        var entity = await _db.FinancialTransactionLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.FinancialTransactionId = request.FinancialTransactionId;
            entity.CostCenterId = request.CostCenterId;
            entity.ProjectId = request.ProjectId;
            entity.Amount = request.Amount;
            entity.Description = request.Description;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.FinancialTransactionLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
