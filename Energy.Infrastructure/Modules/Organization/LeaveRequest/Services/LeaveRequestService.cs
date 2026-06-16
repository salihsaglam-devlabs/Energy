using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Organization.LeaveRequest.Services;
using Energy.Shared.Models.V1.Organization.LeaveRequest.Requests;
using Energy.Shared.Models.V1.Organization.LeaveRequest.Responses;

namespace Energy.Infrastructure.Modules.Organization.LeaveRequest.Services;

/// <summary>LeaveRequest CRUD servisi (projection, pagination, soft-delete).</summary>
public class LeaveRequestService : ILeaveRequestService
{
    private readonly EnergyDbContext _db;

    public LeaveRequestService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<LeaveRequestListResponse>>> GetListAsync(GetLeaveRequestListRequest request, CancellationToken ct = default)
    {
        var query = _db.LeaveRequests.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new LeaveRequestListResponse
            {
                Id = e.Id,
                EmployeeId = e.EmployeeId,
                LeaveType = e.LeaveType,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Days = e.Days,
                Reason = e.Reason,
                Status = e.Status,
                ApprovalRequestId = e.ApprovalRequestId,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<LeaveRequestListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<LeaveRequestListResponse>>.Success(page);
    }

    public async Task<BaseResponse<LeaveRequestDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.LeaveRequests.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new LeaveRequestDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                EmployeeId = e.EmployeeId,
                LeaveType = e.LeaveType,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Days = e.Days,
                Reason = e.Reason,
                Status = e.Status,
                ApprovalRequestId = e.ApprovalRequestId
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<LeaveRequestDetailResponse>.Failure("NotFound")
            : BaseResponse<LeaveRequestDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateLeaveRequestRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Organization.LeaveRequest
        {
            Id = Guid.NewGuid(),
            EmployeeId = request.EmployeeId,
            LeaveType = request.LeaveType,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Days = request.Days,
            Reason = request.Reason,
            Status = request.Status,
            ApprovalRequestId = request.ApprovalRequestId,
            CreatedAt = DateTime.UtcNow,
        };
        _db.LeaveRequests.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateLeaveRequestRequest request, CancellationToken ct = default)
    {
        var entity = await _db.LeaveRequests.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.EmployeeId = request.EmployeeId;
            entity.LeaveType = request.LeaveType;
            entity.StartDate = request.StartDate;
            entity.EndDate = request.EndDate;
            entity.Days = request.Days;
            entity.Reason = request.Reason;
            entity.Status = request.Status;
            entity.ApprovalRequestId = request.ApprovalRequestId;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.LeaveRequests.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
