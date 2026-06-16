using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.BusinessPartners.BusinessPartnerBankAccount.Services;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerBankAccount.Requests;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerBankAccount.Responses;

namespace Energy.Infrastructure.Modules.BusinessPartners.BusinessPartnerBankAccount.Services;

/// <summary>BusinessPartnerBankAccount CRUD servisi (projection, pagination, soft-delete).</summary>
public class BusinessPartnerBankAccountService : IBusinessPartnerBankAccountService
{
    private readonly EnergyDbContext _db;

    public BusinessPartnerBankAccountService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<BusinessPartnerBankAccountListResponse>>> GetListAsync(GetBusinessPartnerBankAccountListRequest request, CancellationToken ct = default)
    {
        var query = _db.BusinessPartnerBankAccounts.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new BusinessPartnerBankAccountListResponse
            {
                Id = e.Id,
                BusinessPartnerId = e.BusinessPartnerId,
                BankName = e.BankName,
                Branch = e.Branch,
                Iban = e.Iban,
                CurrencyId = e.CurrencyId,
                IsPrimary = e.IsPrimary,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<BusinessPartnerBankAccountListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<BusinessPartnerBankAccountListResponse>>.Success(page);
    }

    public async Task<BaseResponse<BusinessPartnerBankAccountDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.BusinessPartnerBankAccounts.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new BusinessPartnerBankAccountDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                BusinessPartnerId = e.BusinessPartnerId,
                BankName = e.BankName,
                Branch = e.Branch,
                Iban = e.Iban,
                CurrencyId = e.CurrencyId,
                IsPrimary = e.IsPrimary
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<BusinessPartnerBankAccountDetailResponse>.Failure("NotFound")
            : BaseResponse<BusinessPartnerBankAccountDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateBusinessPartnerBankAccountRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.BusinessPartners.BusinessPartnerBankAccount
        {
            Id = Guid.NewGuid(),
            BusinessPartnerId = request.BusinessPartnerId,
            BankName = request.BankName,
            Branch = request.Branch,
            Iban = request.Iban,
            CurrencyId = request.CurrencyId,
            IsPrimary = request.IsPrimary,
            CreatedAt = DateTime.UtcNow,
        };
        _db.BusinessPartnerBankAccounts.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateBusinessPartnerBankAccountRequest request, CancellationToken ct = default)
    {
        var entity = await _db.BusinessPartnerBankAccounts.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.BusinessPartnerId = request.BusinessPartnerId;
            entity.BankName = request.BankName;
            entity.Branch = request.Branch;
            entity.Iban = request.Iban;
            entity.CurrencyId = request.CurrencyId;
            entity.IsPrimary = request.IsPrimary;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.BusinessPartnerBankAccounts.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
