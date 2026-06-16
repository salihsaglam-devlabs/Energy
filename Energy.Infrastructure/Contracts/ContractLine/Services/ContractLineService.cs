using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Contracts.ContractLine.Services;
using Energy.Shared.Models.V1.Contracts.ContractLine.Requests;
using Energy.Shared.Models.V1.Contracts.ContractLine.Responses;

namespace Energy.Infrastructure.Contracts.ContractLine.Services;

/// <summary>ContractLine CRUD servisi (projection, pagination, soft-delete).</summary>
public class ContractLineService : IContractLineService
{
    private readonly AppDbContext _db;

    public ContractLineService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ContractLineListResponse>>> GetListAsync(GetContractLineListRequest request, CancellationToken ct = default)
    {
        var query = _db.ContractLines.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ContractLineListResponse
            {
                Id = e.Id,
                ContractId = e.ContractId,
                Description = e.Description,
                Quantity = e.Quantity,
                UnitPrice = e.UnitPrice,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ContractLineListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ContractLineListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ContractLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ContractLines.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ContractLineDetailResponse
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
                Description = e.Description,
                Quantity = e.Quantity,
                UnitPrice = e.UnitPrice
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ContractLineDetailResponse>.Failure("NotFound")
            : BaseResponse<ContractLineDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateContractLineRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Contracts.ContractLine
        {
            Id = Guid.NewGuid(),
            ContractId = request.ContractId,
            Description = request.Description,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ContractLines.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateContractLineRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ContractLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.ContractId = request.ContractId;
            entity.Description = request.Description;
            entity.Quantity = request.Quantity;
            entity.UnitPrice = request.UnitPrice;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ContractLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
