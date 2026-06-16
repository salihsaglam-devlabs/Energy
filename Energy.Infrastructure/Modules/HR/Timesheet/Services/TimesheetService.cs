using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.HR.Timesheet.Services;
using Energy.Shared.Models.V1.HR.Timesheet.Requests;
using Energy.Shared.Models.V1.HR.Timesheet.Responses;

namespace Energy.Infrastructure.Modules.HR.Timesheet.Services;

/// <summary>Timesheet CRUD servisi (projection, pagination, soft-delete).</summary>
public class TimesheetService : ITimesheetService
{
    private readonly EnergyDbContext _db;

    public TimesheetService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<TimesheetListResponse>>> GetListAsync(GetTimesheetListRequest request, CancellationToken ct = default)
    {
        var query = _db.Timesheets.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new TimesheetListResponse
            {
                Id = e.Id,
                TimesheetNo = e.TimesheetNo,
                PeriodStart = e.PeriodStart,
                PeriodEnd = e.PeriodEnd,
                Status = e.Status,
                ApprovalRequestId = e.ApprovalRequestId,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<TimesheetListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<TimesheetListResponse>>.Success(page);
    }

    public async Task<BaseResponse<TimesheetDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.Timesheets.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new TimesheetDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                TimesheetNo = e.TimesheetNo,
                PeriodStart = e.PeriodStart,
                PeriodEnd = e.PeriodEnd,
                Status = e.Status,
                ApprovalRequestId = e.ApprovalRequestId
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<TimesheetDetailResponse>.Failure("NotFound")
            : BaseResponse<TimesheetDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateTimesheetRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.HR.Timesheet
        {
            Id = Guid.NewGuid(),
            TimesheetNo = request.TimesheetNo,
            PeriodStart = request.PeriodStart,
            PeriodEnd = request.PeriodEnd,
            Status = request.Status,
            ApprovalRequestId = request.ApprovalRequestId,
            CreatedAt = DateTime.UtcNow,
        };
        _db.Timesheets.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateTimesheetRequest request, CancellationToken ct = default)
    {
        var entity = await _db.Timesheets.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.TimesheetNo = request.TimesheetNo;
            entity.PeriodStart = request.PeriodStart;
            entity.PeriodEnd = request.PeriodEnd;
            entity.Status = request.Status;
            entity.ApprovalRequestId = request.ApprovalRequestId;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.Timesheets.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
