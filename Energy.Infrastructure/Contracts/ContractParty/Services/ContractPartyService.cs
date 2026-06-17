using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Contracts.ContractParty.Services;
using Energy.Shared.Models.V1.Contracts.ContractParty.Requests;
using Energy.Shared.Models.V1.Contracts.ContractParty.Responses;

namespace Energy.Infrastructure.Contracts.ContractParty.Services;

/// <summary>ContractParty CRUD servisi (projection, pagination, soft-delete).</summary>
public class ContractPartyService : IContractPartyService
{
    private readonly AppDbContext _db;

    public ContractPartyService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ContractPartyListResponse>>> GetListAsync(GetContractPartyListRequest request, CancellationToken ct = default)
    {
        var query = _db.ContractParties.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ContractPartyListResponse
            {
                Id = e.Id,
                ContractId = e.ContractId,
                BusinessPartnerId = e.BusinessPartnerId,
                PartyRole = e.PartyRole,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ContractPartyListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ContractPartyListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ContractPartyDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ContractParties.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ContractPartyDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                ContractId = e.ContractId,
                BusinessPartnerId = e.BusinessPartnerId,
                PartyRole = e.PartyRole
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ContractPartyDetailResponse>.Failure("NotFound")
            : BaseResponse<ContractPartyDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateContractPartyRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Contracts.ContractParty
        {
            Id = Guid.NewGuid(),
            ContractId = request.ContractId,
            BusinessPartnerId = request.BusinessPartnerId,
            PartyRole = request.PartyRole,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ContractParties.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateContractPartyRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ContractParties.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.ContractId = request.ContractId;
            entity.BusinessPartnerId = request.BusinessPartnerId;
            entity.PartyRole = request.PartyRole;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ContractParties.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
