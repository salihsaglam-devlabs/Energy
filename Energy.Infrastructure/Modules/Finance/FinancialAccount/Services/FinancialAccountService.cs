using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Finance.FinancialAccount.Services;
using Energy.Shared.Models.V1.Finance.FinancialAccount.Requests;
using Energy.Shared.Models.V1.Finance.FinancialAccount.Responses;

namespace Energy.Infrastructure.Modules.Finance.FinancialAccount.Services;

/// <summary>FinancialAccount CRUD servisi (projection, pagination, soft-delete).</summary>
public class FinancialAccountService : IFinancialAccountService
{
    private readonly EnergyDbContext _db;

    public FinancialAccountService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<FinancialAccountListResponse>>> GetListAsync(GetFinancialAccountListRequest request, CancellationToken ct = default)
    {
        var query = _db.FinancialAccounts.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new FinancialAccountListResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                AccountType = e.AccountType,
                CurrencyId = e.CurrencyId,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<FinancialAccountListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<FinancialAccountListResponse>>.Success(page);
    }

    public async Task<BaseResponse<FinancialAccountDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.FinancialAccounts.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new FinancialAccountDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                Code = e.Code,
                Name = e.Name,
                AccountType = e.AccountType,
                CurrencyId = e.CurrencyId,
                IsActive = e.IsActive
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<FinancialAccountDetailResponse>.Failure("NotFound")
            : BaseResponse<FinancialAccountDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateFinancialAccountRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Finance.FinancialAccount
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            Name = request.Name,
            AccountType = request.AccountType,
            CurrencyId = request.CurrencyId,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
        };
        _db.FinancialAccounts.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateFinancialAccountRequest request, CancellationToken ct = default)
    {
        var entity = await _db.FinancialAccounts.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.Code = request.Code;
            entity.Name = request.Name;
            entity.AccountType = request.AccountType;
            entity.CurrencyId = request.CurrencyId;
            entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.FinancialAccounts.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
