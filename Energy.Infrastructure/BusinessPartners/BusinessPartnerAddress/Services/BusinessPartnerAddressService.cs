using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.BusinessPartners.BusinessPartnerAddress.Services;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerAddress.Requests;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerAddress.Responses;

namespace Energy.Infrastructure.BusinessPartners.BusinessPartnerAddress.Services;

/// <summary>BusinessPartnerAddress CRUD servisi (projection, pagination, soft-delete).</summary>
public class BusinessPartnerAddressService : IBusinessPartnerAddressService
{
    private readonly AppDbContext _db;

    public BusinessPartnerAddressService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<BusinessPartnerAddressListResponse>>> GetListAsync(GetBusinessPartnerAddressListRequest request, CancellationToken ct = default)
    {
        var query = _db.BusinessPartnerAddresses.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new BusinessPartnerAddressListResponse
            {
                Id = e.Id,
                BusinessPartnerId = e.BusinessPartnerId,
                AddressType = e.AddressType,
                AddressLine = e.AddressLine,
                City = e.City,
                Country = e.Country,
                PostalCode = e.PostalCode,
                IsPrimary = e.IsPrimary,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<BusinessPartnerAddressListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<BusinessPartnerAddressListResponse>>.Success(page);
    }

    public async Task<BaseResponse<BusinessPartnerAddressDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.BusinessPartnerAddresses.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new BusinessPartnerAddressDetailResponse
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
                AddressType = e.AddressType,
                AddressLine = e.AddressLine,
                City = e.City,
                Country = e.Country,
                PostalCode = e.PostalCode,
                IsPrimary = e.IsPrimary
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<BusinessPartnerAddressDetailResponse>.Failure("NotFound")
            : BaseResponse<BusinessPartnerAddressDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateBusinessPartnerAddressRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.BusinessPartners.BusinessPartnerAddress
        {
            Id = Guid.NewGuid(),
            BusinessPartnerId = request.BusinessPartnerId,
            AddressType = request.AddressType,
            AddressLine = request.AddressLine,
            City = request.City,
            Country = request.Country,
            PostalCode = request.PostalCode,
            IsPrimary = request.IsPrimary,
            CreatedAt = DateTime.UtcNow,
        };
        _db.BusinessPartnerAddresses.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateBusinessPartnerAddressRequest request, CancellationToken ct = default)
    {
        var entity = await _db.BusinessPartnerAddresses.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.BusinessPartnerId = request.BusinessPartnerId;
            entity.AddressType = request.AddressType;
            entity.AddressLine = request.AddressLine;
            entity.City = request.City;
            entity.Country = request.Country;
            entity.PostalCode = request.PostalCode;
            entity.IsPrimary = request.IsPrimary;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.BusinessPartnerAddresses.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
