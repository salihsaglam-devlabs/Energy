using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.HR.TimesheetLine.Services;
using Energy.Shared.Models.V1.HR.TimesheetLine.Requests;
using Energy.Shared.Models.V1.HR.TimesheetLine.Responses;

namespace Energy.Infrastructure.Modules.HR.TimesheetLine.Services;

/// <summary>TimesheetLine CRUD servisi (projection, pagination, soft-delete).</summary>
public class TimesheetLineService : ITimesheetLineService
{
    private readonly EnergyDbContext _db;

    public TimesheetLineService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<TimesheetLineListResponse>>> GetListAsync(GetTimesheetLineListRequest request, CancellationToken ct = default)
    {
        var query = _db.TimesheetLines.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new TimesheetLineListResponse
            {
                Id = e.Id,
                TimesheetId = e.TimesheetId,
                EmployeeId = e.EmployeeId,
                ProjectId = e.ProjectId,
                WorkOrderId = e.WorkOrderId,
                WorkDate = e.WorkDate,
                NormalHours = e.NormalHours,
                OvertimeHours = e.OvertimeHours,
                HourlyCost = e.HourlyCost,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<TimesheetLineListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<TimesheetLineListResponse>>.Success(page);
    }

    public async Task<BaseResponse<TimesheetLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.TimesheetLines.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new TimesheetLineDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                TimesheetId = e.TimesheetId,
                EmployeeId = e.EmployeeId,
                ProjectId = e.ProjectId,
                WorkOrderId = e.WorkOrderId,
                WorkDate = e.WorkDate,
                NormalHours = e.NormalHours,
                OvertimeHours = e.OvertimeHours,
                HourlyCost = e.HourlyCost
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<TimesheetLineDetailResponse>.Failure("NotFound")
            : BaseResponse<TimesheetLineDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateTimesheetLineRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.HR.TimesheetLine
        {
            Id = Guid.NewGuid(),
            TimesheetId = request.TimesheetId,
            EmployeeId = request.EmployeeId,
            ProjectId = request.ProjectId,
            WorkOrderId = request.WorkOrderId,
            WorkDate = request.WorkDate,
            NormalHours = request.NormalHours,
            OvertimeHours = request.OvertimeHours,
            HourlyCost = request.HourlyCost,
            CreatedAt = DateTime.UtcNow,
        };
        _db.TimesheetLines.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateTimesheetLineRequest request, CancellationToken ct = default)
    {
        var entity = await _db.TimesheetLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.TimesheetId = request.TimesheetId;
            entity.EmployeeId = request.EmployeeId;
            entity.ProjectId = request.ProjectId;
            entity.WorkOrderId = request.WorkOrderId;
            entity.WorkDate = request.WorkDate;
            entity.NormalHours = request.NormalHours;
            entity.OvertimeHours = request.OvertimeHours;
            entity.HourlyCost = request.HourlyCost;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.TimesheetLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
