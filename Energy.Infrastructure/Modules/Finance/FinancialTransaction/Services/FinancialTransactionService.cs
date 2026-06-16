using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Finance.FinancialTransaction.Services;
using Energy.Shared.Models.V1.Finance.FinancialTransaction.Requests;
using Energy.Shared.Models.V1.Finance.FinancialTransaction.Responses;

namespace Energy.Infrastructure.Modules.Finance.FinancialTransaction.Services;

/// <summary>FinancialTransaction CRUD servisi (projection, pagination, soft-delete).</summary>
public class FinancialTransactionService : IFinancialTransactionService
{
    private readonly AppDbContext _db;

    public FinancialTransactionService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<FinancialTransactionListResponse>>> GetListAsync(GetFinancialTransactionListRequest request, CancellationToken ct = default)
    {
        var query = _db.FinancialTransactions.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new FinancialTransactionListResponse
            {
                Id = e.Id,
                TransactionType = e.TransactionType,
                ProjectId = e.ProjectId,
                PartnerId = e.PartnerId,
                CurrencyId = e.CurrencyId,
                Amount = e.Amount,
                RelatedModule = e.RelatedModule,
                RelatedEntityType = e.RelatedEntityType,
                RelatedEntityId = e.RelatedEntityId,
                FinancialAccountId = e.FinancialAccountId,
                CostCenterId = e.CostCenterId,
                TransactionDate = e.TransactionDate,
                Description = e.Description,
                IsReversed = e.IsReversed,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<FinancialTransactionListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<FinancialTransactionListResponse>>.Success(page);
    }

    public async Task<BaseResponse<FinancialTransactionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.FinancialTransactions.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new FinancialTransactionDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                TransactionType = e.TransactionType,
                ProjectId = e.ProjectId,
                PartnerId = e.PartnerId,
                CurrencyId = e.CurrencyId,
                Amount = e.Amount,
                RelatedModule = e.RelatedModule,
                RelatedEntityType = e.RelatedEntityType,
                RelatedEntityId = e.RelatedEntityId,
                FinancialAccountId = e.FinancialAccountId,
                CostCenterId = e.CostCenterId,
                TransactionDate = e.TransactionDate,
                Description = e.Description,
                IsReversed = e.IsReversed
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<FinancialTransactionDetailResponse>.Failure("NotFound")
            : BaseResponse<FinancialTransactionDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateFinancialTransactionRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Finance.FinancialTransaction
        {
            Id = Guid.NewGuid(),
            TransactionType = request.TransactionType,
            ProjectId = request.ProjectId,
            PartnerId = request.PartnerId,
            CurrencyId = request.CurrencyId,
            Amount = request.Amount,
            RelatedModule = request.RelatedModule,
            RelatedEntityType = request.RelatedEntityType,
            RelatedEntityId = request.RelatedEntityId,
            FinancialAccountId = request.FinancialAccountId,
            CostCenterId = request.CostCenterId,
            TransactionDate = request.TransactionDate,
            Description = request.Description,
            IsReversed = request.IsReversed,
            CreatedAt = DateTime.UtcNow,
        };
        _db.FinancialTransactions.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateFinancialTransactionRequest request, CancellationToken ct = default)
    {
        var entity = await _db.FinancialTransactions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.TransactionType = request.TransactionType;
            entity.ProjectId = request.ProjectId;
            entity.PartnerId = request.PartnerId;
            entity.CurrencyId = request.CurrencyId;
            entity.Amount = request.Amount;
            entity.RelatedModule = request.RelatedModule;
            entity.RelatedEntityType = request.RelatedEntityType;
            entity.RelatedEntityId = request.RelatedEntityId;
            entity.FinancialAccountId = request.FinancialAccountId;
            entity.CostCenterId = request.CostCenterId;
            entity.TransactionDate = request.TransactionDate;
            entity.Description = request.Description;
            entity.IsReversed = request.IsReversed;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.FinancialTransactions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
