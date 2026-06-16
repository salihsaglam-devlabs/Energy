using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Operations.WorkOrderAssignment.Services;
using Energy.Shared.Models.V1.Operations.WorkOrderAssignment.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderAssignment.Responses;

namespace Energy.Infrastructure.Modules.Operations.WorkOrderAssignment.Services;

/// <summary>WorkOrderAssignment CRUD servisi (projection, pagination, soft-delete).</summary>
public class WorkOrderAssignmentService : IWorkOrderAssignmentService
{
    private readonly EnergyDbContext _db;

    public WorkOrderAssignmentService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<WorkOrderAssignmentListResponse>>> GetListAsync(GetWorkOrderAssignmentListRequest request, CancellationToken ct = default)
    {
        var query = _db.WorkOrderAssignments.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new WorkOrderAssignmentListResponse
            {
                Id = e.Id,
                WorkOrderId = e.WorkOrderId,
                EmployeeId = e.EmployeeId,
                UserId = e.UserId,
                AssignmentRole = e.AssignmentRole,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<WorkOrderAssignmentListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<WorkOrderAssignmentListResponse>>.Success(page);
    }

    public async Task<BaseResponse<WorkOrderAssignmentDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.WorkOrderAssignments.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new WorkOrderAssignmentDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                WorkOrderId = e.WorkOrderId,
                EmployeeId = e.EmployeeId,
                UserId = e.UserId,
                AssignmentRole = e.AssignmentRole
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<WorkOrderAssignmentDetailResponse>.Failure("NotFound")
            : BaseResponse<WorkOrderAssignmentDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateWorkOrderAssignmentRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Operations.WorkOrderAssignment
        {
            Id = Guid.NewGuid(),
            WorkOrderId = request.WorkOrderId,
            EmployeeId = request.EmployeeId,
            UserId = request.UserId,
            AssignmentRole = request.AssignmentRole,
            CreatedAt = DateTime.UtcNow,
        };
        _db.WorkOrderAssignments.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateWorkOrderAssignmentRequest request, CancellationToken ct = default)
    {
        var entity = await _db.WorkOrderAssignments.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.WorkOrderId = request.WorkOrderId;
            entity.EmployeeId = request.EmployeeId;
            entity.UserId = request.UserId;
            entity.AssignmentRole = request.AssignmentRole;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.WorkOrderAssignments.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
