using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Requests.RequestLine.Services;
using Energy.Shared.Models.V1.Requests.RequestLine.Requests;
using Energy.Shared.Models.V1.Requests.RequestLine.Responses;

namespace Energy.Infrastructure.Modules.Requests.RequestLine.Services;

/// <summary>RequestLine CRUD servisi (projection, pagination, soft-delete).</summary>
public class RequestLineService : IRequestLineService
{
    private readonly EnergyDbContext _db;

    public RequestLineService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<RequestLineListResponse>>> GetListAsync(GetRequestLineListRequest request, CancellationToken ct = default)
    {
        var query = _db.RequestLines.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new RequestLineListResponse
            {
                Id = e.Id,
                RequestId = e.RequestId,
                MaterialId = e.MaterialId,
                RequestedMaterialText = e.RequestedMaterialText,
                Quantity = e.Quantity,
                UnitOfMeasureId = e.UnitOfMeasureId,
                Note = e.Note,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<RequestLineListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<RequestLineListResponse>>.Success(page);
    }

    public async Task<BaseResponse<RequestLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.RequestLines.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new RequestLineDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                RequestId = e.RequestId,
                MaterialId = e.MaterialId,
                RequestedMaterialText = e.RequestedMaterialText,
                Quantity = e.Quantity,
                UnitOfMeasureId = e.UnitOfMeasureId,
                Note = e.Note
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<RequestLineDetailResponse>.Failure("NotFound")
            : BaseResponse<RequestLineDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateRequestLineRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Requests.RequestLine
        {
            Id = Guid.NewGuid(),
            RequestId = request.RequestId,
            MaterialId = request.MaterialId,
            RequestedMaterialText = request.RequestedMaterialText,
            Quantity = request.Quantity,
            UnitOfMeasureId = request.UnitOfMeasureId,
            Note = request.Note,
            CreatedAt = DateTime.UtcNow,
        };
        _db.RequestLines.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateRequestLineRequest request, CancellationToken ct = default)
    {
        var entity = await _db.RequestLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.RequestId = request.RequestId;
            entity.MaterialId = request.MaterialId;
            entity.RequestedMaterialText = request.RequestedMaterialText;
            entity.Quantity = request.Quantity;
            entity.UnitOfMeasureId = request.UnitOfMeasureId;
            entity.Note = request.Note;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.RequestLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
