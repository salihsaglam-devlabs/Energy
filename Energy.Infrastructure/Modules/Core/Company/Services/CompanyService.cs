using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Core.Company.Services;
using Energy.Shared.Models.V1.Core.Company.Requests;
using Energy.Shared.Models.V1.Core.Company.Responses;

namespace Energy.Infrastructure.Modules.Core.Company.Services;

/// <summary>Company CRUD servisi (projection, pagination, soft-delete).</summary>
public class CompanyService : ICompanyService
{
    private readonly AppDbContext _db;

    public CompanyService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<CompanyListResponse>>> GetListAsync(GetCompanyListRequest request, CancellationToken ct = default)
    {
        var query = _db.Companies.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new CompanyListResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                BaseCurrencyId = e.BaseCurrencyId,
                TaxNumber = e.TaxNumber,
                Address = e.Address,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<CompanyListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<CompanyListResponse>>.Success(page);
    }

    public async Task<BaseResponse<CompanyDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.Companies.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new CompanyDetailResponse
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
                BaseCurrencyId = e.BaseCurrencyId,
                TaxNumber = e.TaxNumber,
                Address = e.Address,
                IsActive = e.IsActive
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<CompanyDetailResponse>.Failure("NotFound")
            : BaseResponse<CompanyDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateCompanyRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Core.Company
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            Name = request.Name,
            BaseCurrencyId = request.BaseCurrencyId,
            TaxNumber = request.TaxNumber,
            Address = request.Address,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
        };
        _db.Companies.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateCompanyRequest request, CancellationToken ct = default)
    {
        var entity = await _db.Companies.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.Code = request.Code;
            entity.Name = request.Name;
            entity.BaseCurrencyId = request.BaseCurrencyId;
            entity.TaxNumber = request.TaxNumber;
            entity.Address = request.Address;
            entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.Companies.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
