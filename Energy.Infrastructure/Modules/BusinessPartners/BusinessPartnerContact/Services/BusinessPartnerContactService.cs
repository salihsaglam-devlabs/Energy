using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.BusinessPartners.BusinessPartnerContact.Services;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerContact.Requests;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerContact.Responses;

namespace Energy.Infrastructure.Modules.BusinessPartners.BusinessPartnerContact.Services;

/// <summary>BusinessPartnerContact CRUD servisi (projection, pagination, soft-delete).</summary>
public class BusinessPartnerContactService : IBusinessPartnerContactService
{
    private readonly EnergyDbContext _db;

    public BusinessPartnerContactService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<BusinessPartnerContactListResponse>>> GetListAsync(GetBusinessPartnerContactListRequest request, CancellationToken ct = default)
    {
        var query = _db.BusinessPartnerContacts.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new BusinessPartnerContactListResponse
            {
                Id = e.Id,
                BusinessPartnerId = e.BusinessPartnerId,
                FullName = e.FullName,
                Title = e.Title,
                Phone = e.Phone,
                Email = e.Email,
                IsPrimary = e.IsPrimary,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<BusinessPartnerContactListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<BusinessPartnerContactListResponse>>.Success(page);
    }

    public async Task<BaseResponse<BusinessPartnerContactDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.BusinessPartnerContacts.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new BusinessPartnerContactDetailResponse
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
                FullName = e.FullName,
                Title = e.Title,
                Phone = e.Phone,
                Email = e.Email,
                IsPrimary = e.IsPrimary
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<BusinessPartnerContactDetailResponse>.Failure("NotFound")
            : BaseResponse<BusinessPartnerContactDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateBusinessPartnerContactRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.BusinessPartners.BusinessPartnerContact
        {
            Id = Guid.NewGuid(),
            BusinessPartnerId = request.BusinessPartnerId,
            FullName = request.FullName,
            Title = request.Title,
            Phone = request.Phone,
            Email = request.Email,
            IsPrimary = request.IsPrimary,
            CreatedAt = DateTime.UtcNow,
        };
        _db.BusinessPartnerContacts.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateBusinessPartnerContactRequest request, CancellationToken ct = default)
    {
        var entity = await _db.BusinessPartnerContacts.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.BusinessPartnerId = request.BusinessPartnerId;
            entity.FullName = request.FullName;
            entity.Title = request.Title;
            entity.Phone = request.Phone;
            entity.Email = request.Email;
            entity.IsPrimary = request.IsPrimary;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.BusinessPartnerContacts.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
