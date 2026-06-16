using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.BusinessPartners.BusinessPartner.Services;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartner.Requests;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartner.Responses;

namespace Energy.Infrastructure.Modules.BusinessPartners.BusinessPartner.Services;

/// <summary>BusinessPartner CRUD servisi (projection, pagination, soft-delete).</summary>
public class BusinessPartnerService : IBusinessPartnerService
{
    private readonly EnergyDbContext _db;

    public BusinessPartnerService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<BusinessPartnerListResponse>>> GetListAsync(GetBusinessPartnerListRequest request, CancellationToken ct = default)
    {
        var query = _db.BusinessPartners.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new BusinessPartnerListResponse
            {
                Id = e.Id,
                PartnerType = e.PartnerType,
                Code = e.Code,
                Name = e.Name,
                TaxNumber = e.TaxNumber,
                TaxOffice = e.TaxOffice,
                Phone = e.Phone,
                Email = e.Email,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<BusinessPartnerListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<BusinessPartnerListResponse>>.Success(page);
    }

    public async Task<BaseResponse<BusinessPartnerDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.BusinessPartners.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new BusinessPartnerDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                PartnerType = e.PartnerType,
                Code = e.Code,
                Name = e.Name,
                TaxNumber = e.TaxNumber,
                TaxOffice = e.TaxOffice,
                Phone = e.Phone,
                Email = e.Email,
                IsActive = e.IsActive
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<BusinessPartnerDetailResponse>.Failure("NotFound")
            : BaseResponse<BusinessPartnerDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateBusinessPartnerRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.BusinessPartners.BusinessPartner
        {
            Id = Guid.NewGuid(),
            PartnerType = request.PartnerType,
            Code = request.Code,
            Name = request.Name,
            TaxNumber = request.TaxNumber,
            TaxOffice = request.TaxOffice,
            Phone = request.Phone,
            Email = request.Email,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
        };
        _db.BusinessPartners.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateBusinessPartnerRequest request, CancellationToken ct = default)
    {
        var entity = await _db.BusinessPartners.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.PartnerType = request.PartnerType;
            entity.Code = request.Code;
            entity.Name = request.Name;
            entity.TaxNumber = request.TaxNumber;
            entity.TaxOffice = request.TaxOffice;
            entity.Phone = request.Phone;
            entity.Email = request.Email;
            entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.BusinessPartners.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
