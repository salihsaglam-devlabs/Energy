using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.FieldOperations.DailySiteReportWorker.Services;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportWorker.Requests;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportWorker.Responses;

namespace Energy.Infrastructure.Modules.FieldOperations.DailySiteReportWorker.Services;

/// <summary>DailySiteReportWorker CRUD servisi (projection, pagination, soft-delete).</summary>
public class DailySiteReportWorkerService : IDailySiteReportWorkerService
{
    private readonly EnergyDbContext _db;

    public DailySiteReportWorkerService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<DailySiteReportWorkerListResponse>>> GetListAsync(GetDailySiteReportWorkerListRequest request, CancellationToken ct = default)
    {
        var query = _db.DailySiteReportWorkers.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new DailySiteReportWorkerListResponse
            {
                Id = e.Id,
                DailySiteReportId = e.DailySiteReportId,
                EmployeeId = e.EmployeeId,
                HoursWorked = e.HoursWorked,
                Note = e.Note,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<DailySiteReportWorkerListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<DailySiteReportWorkerListResponse>>.Success(page);
    }

    public async Task<BaseResponse<DailySiteReportWorkerDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.DailySiteReportWorkers.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new DailySiteReportWorkerDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                DailySiteReportId = e.DailySiteReportId,
                EmployeeId = e.EmployeeId,
                HoursWorked = e.HoursWorked,
                Note = e.Note
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<DailySiteReportWorkerDetailResponse>.Failure("NotFound")
            : BaseResponse<DailySiteReportWorkerDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateDailySiteReportWorkerRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.FieldOperations.DailySiteReportWorker
        {
            Id = Guid.NewGuid(),
            DailySiteReportId = request.DailySiteReportId,
            EmployeeId = request.EmployeeId,
            HoursWorked = request.HoursWorked,
            Note = request.Note,
            CreatedAt = DateTime.UtcNow,
        };
        _db.DailySiteReportWorkers.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDailySiteReportWorkerRequest request, CancellationToken ct = default)
    {
        var entity = await _db.DailySiteReportWorkers.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.DailySiteReportId = request.DailySiteReportId;
            entity.EmployeeId = request.EmployeeId;
            entity.HoursWorked = request.HoursWorked;
            entity.Note = request.Note;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.DailySiteReportWorkers.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
