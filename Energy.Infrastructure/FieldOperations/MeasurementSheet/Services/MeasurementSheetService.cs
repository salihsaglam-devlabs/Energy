using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.FieldOperations.MeasurementSheet.Services;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheet.Requests;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheet.Responses;

namespace Energy.Infrastructure.FieldOperations.MeasurementSheet.Services;

/// <summary>MeasurementSheet CRUD servisi (projection, pagination, soft-delete).</summary>
public class MeasurementSheetService : IMeasurementSheetService
{
    private readonly AppDbContext _db;

    public MeasurementSheetService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<MeasurementSheetListResponse>>> GetListAsync(GetMeasurementSheetListRequest request, CancellationToken ct = default)
    {
        var query = _db.MeasurementSheets.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new MeasurementSheetListResponse
            {
                Id = e.Id,
                ProjectId = e.ProjectId,
                ContractId = e.ContractId,
                SheetNo = e.SheetNo,
                SheetDate = e.SheetDate,
                Status = e.Status,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<MeasurementSheetListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<MeasurementSheetListResponse>>.Success(page);
    }

    public async Task<BaseResponse<MeasurementSheetDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.MeasurementSheets.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new MeasurementSheetDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                ProjectId = e.ProjectId,
                ContractId = e.ContractId,
                SheetNo = e.SheetNo,
                SheetDate = e.SheetDate,
                Status = e.Status
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<MeasurementSheetDetailResponse>.Failure("NotFound")
            : BaseResponse<MeasurementSheetDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateMeasurementSheetRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.FieldOperations.MeasurementSheet
        {
            Id = Guid.NewGuid(),
            ProjectId = request.ProjectId,
            ContractId = request.ContractId,
            SheetNo = request.SheetNo,
            SheetDate = request.SheetDate,
            Status = request.Status,
            CreatedAt = DateTime.UtcNow,
        };
        _db.MeasurementSheets.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateMeasurementSheetRequest request, CancellationToken ct = default)
    {
        var entity = await _db.MeasurementSheets.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.ProjectId = request.ProjectId;
            entity.ContractId = request.ContractId;
            entity.SheetNo = request.SheetNo;
            entity.SheetDate = request.SheetDate;
            entity.Status = request.Status;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.MeasurementSheets.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
