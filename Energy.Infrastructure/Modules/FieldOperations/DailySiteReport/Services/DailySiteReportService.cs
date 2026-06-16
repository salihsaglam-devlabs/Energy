using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.FieldOperations.DailySiteReport.Services;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReport.Requests;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReport.Responses;

namespace Energy.Infrastructure.Modules.FieldOperations.DailySiteReport.Services;

/// <summary>DailySiteReport CRUD servisi (projection, pagination, soft-delete).</summary>
public class DailySiteReportService : IDailySiteReportService
{
    private readonly AppDbContext _db;

    public DailySiteReportService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<DailySiteReportListResponse>>> GetListAsync(GetDailySiteReportListRequest request, CancellationToken ct = default)
    {
        var query = _db.DailySiteReports.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new DailySiteReportListResponse
            {
                Id = e.Id,
                ProjectId = e.ProjectId,
                WorkOrderId = e.WorkOrderId,
                ReportNo = e.ReportNo,
                ReportDate = e.ReportDate,
                Weather = e.Weather,
                Notes = e.Notes,
                Status = e.Status,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<DailySiteReportListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<DailySiteReportListResponse>>.Success(page);
    }

    public async Task<BaseResponse<DailySiteReportDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.DailySiteReports.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new DailySiteReportDetailResponse
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
                WorkOrderId = e.WorkOrderId,
                ReportNo = e.ReportNo,
                ReportDate = e.ReportDate,
                Weather = e.Weather,
                Notes = e.Notes,
                Status = e.Status
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<DailySiteReportDetailResponse>.Failure("NotFound")
            : BaseResponse<DailySiteReportDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateDailySiteReportRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.FieldOperations.DailySiteReport
        {
            Id = Guid.NewGuid(),
            ProjectId = request.ProjectId,
            WorkOrderId = request.WorkOrderId,
            ReportNo = request.ReportNo,
            ReportDate = request.ReportDate,
            Weather = request.Weather,
            Notes = request.Notes,
            Status = request.Status,
            CreatedAt = DateTime.UtcNow,
        };
        _db.DailySiteReports.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDailySiteReportRequest request, CancellationToken ct = default)
    {
        var entity = await _db.DailySiteReports.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.ProjectId = request.ProjectId;
            entity.WorkOrderId = request.WorkOrderId;
            entity.ReportNo = request.ReportNo;
            entity.ReportDate = request.ReportDate;
            entity.Weather = request.Weather;
            entity.Notes = request.Notes;
            entity.Status = request.Status;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.DailySiteReports.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
