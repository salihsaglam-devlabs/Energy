using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Organization.EmployeeSkillAssignment.Services;
using Energy.Shared.Models.V1.Organization.EmployeeSkillAssignment.Requests;
using Energy.Shared.Models.V1.Organization.EmployeeSkillAssignment.Responses;

namespace Energy.Infrastructure.Modules.Organization.EmployeeSkillAssignment.Services;

/// <summary>EmployeeSkillAssignment CRUD servisi (projection, pagination, soft-delete).</summary>
public class EmployeeSkillAssignmentService : IEmployeeSkillAssignmentService
{
    private readonly AppDbContext _db;

    public EmployeeSkillAssignmentService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<EmployeeSkillAssignmentListResponse>>> GetListAsync(GetEmployeeSkillAssignmentListRequest request, CancellationToken ct = default)
    {
        var query = _db.EmployeeSkillAssignments.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new EmployeeSkillAssignmentListResponse
            {
                Id = e.Id,
                EmployeeId = e.EmployeeId,
                EmployeeSkillId = e.EmployeeSkillId,
                Level = e.Level,
                Note = e.Note,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<EmployeeSkillAssignmentListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<EmployeeSkillAssignmentListResponse>>.Success(page);
    }

    public async Task<BaseResponse<EmployeeSkillAssignmentDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.EmployeeSkillAssignments.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new EmployeeSkillAssignmentDetailResponse
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
                EmployeeSkillId = e.EmployeeSkillId,
                Level = e.Level,
                Note = e.Note
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<EmployeeSkillAssignmentDetailResponse>.Failure("NotFound")
            : BaseResponse<EmployeeSkillAssignmentDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateEmployeeSkillAssignmentRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Organization.EmployeeSkillAssignment
        {
            Id = Guid.NewGuid(),
            EmployeeId = request.EmployeeId,
            EmployeeSkillId = request.EmployeeSkillId,
            Level = request.Level,
            Note = request.Note,
            CreatedAt = DateTime.UtcNow,
        };
        _db.EmployeeSkillAssignments.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateEmployeeSkillAssignmentRequest request, CancellationToken ct = default)
    {
        var entity = await _db.EmployeeSkillAssignments.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.EmployeeId = request.EmployeeId;
            entity.EmployeeSkillId = request.EmployeeSkillId;
            entity.Level = request.Level;
            entity.Note = request.Note;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.EmployeeSkillAssignments.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
