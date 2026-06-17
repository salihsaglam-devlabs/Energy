using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Contracts.ContractAmendment.Services;
using Energy.Shared.Models.V1.Contracts.ContractAmendment.Requests;
using Energy.Shared.Models.V1.Contracts.ContractAmendment.Responses;

namespace Energy.Infrastructure.Contracts.ContractAmendment.Services;

/// <summary>ContractAmendment CRUD servisi (projection, pagination, soft-delete).</summary>
public class ContractAmendmentService : IContractAmendmentService
{
    private readonly AppDbContext _db;

    public ContractAmendmentService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ContractAmendmentListResponse>>> GetListAsync(GetContractAmendmentListRequest request, CancellationToken ct = default)
    {
        var query = _db.ContractAmendments.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ContractAmendmentListResponse
            {
                Id = e.Id,
                ContractId = e.ContractId,
                AmendmentNo = e.AmendmentNo,
                AmendmentDate = e.AmendmentDate,
                Description = e.Description,
                AmountDelta = e.AmountDelta,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ContractAmendmentListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ContractAmendmentListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ContractAmendmentDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ContractAmendments.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ContractAmendmentDetailResponse
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
                AmendmentNo = e.AmendmentNo,
                AmendmentDate = e.AmendmentDate,
                Description = e.Description,
                AmountDelta = e.AmountDelta
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ContractAmendmentDetailResponse>.Failure("NotFound")
            : BaseResponse<ContractAmendmentDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateContractAmendmentRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Contracts.ContractAmendment
        {
            Id = Guid.NewGuid(),
            ContractId = request.ContractId,
            AmendmentNo = request.AmendmentNo,
            AmendmentDate = request.AmendmentDate,
            Description = request.Description,
            AmountDelta = request.AmountDelta,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ContractAmendments.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateContractAmendmentRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ContractAmendments.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.ContractId = request.ContractId;
            entity.AmendmentNo = request.AmendmentNo;
            entity.AmendmentDate = request.AmendmentDate;
            entity.Description = request.Description;
            entity.AmountDelta = request.AmountDelta;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ContractAmendments.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
