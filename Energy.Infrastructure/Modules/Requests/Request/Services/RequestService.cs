using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Requests.Request.Services;
using Energy.Shared.Models.V1.Requests.Request.Requests;
using Energy.Shared.Models.V1.Requests.Request.Responses;

namespace Energy.Infrastructure.Modules.Requests.Request.Services;

/// <summary>Request CRUD servisi (projection, pagination, soft-delete).</summary>
public class RequestService : IRequestService
{
    private readonly EnergyDbContext _db;

    public RequestService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<RequestListResponse>>> GetListAsync(GetRequestListRequest request, CancellationToken ct = default)
    {
        var query = _db.Requests.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new RequestListResponse
            {
                Id = e.Id,
                RequestTypeId = e.RequestTypeId,
                ProjectId = e.ProjectId,
                RequestedByUserId = e.RequestedByUserId,
                Status = e.Status,
                RequestNo = e.RequestNo,
                RequestDate = e.RequestDate,
                Description = e.Description,
                ApprovalRequestId = e.ApprovalRequestId,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<RequestListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<RequestListResponse>>.Success(page);
    }

    public async Task<BaseResponse<RequestDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.Requests.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new RequestDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                RequestTypeId = e.RequestTypeId,
                ProjectId = e.ProjectId,
                RequestedByUserId = e.RequestedByUserId,
                Status = e.Status,
                RequestNo = e.RequestNo,
                RequestDate = e.RequestDate,
                Description = e.Description,
                ApprovalRequestId = e.ApprovalRequestId
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<RequestDetailResponse>.Failure("NotFound")
            : BaseResponse<RequestDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateRequestRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Requests.Request
        {
            Id = Guid.NewGuid(),
            RequestTypeId = request.RequestTypeId,
            ProjectId = request.ProjectId,
            RequestedByUserId = request.RequestedByUserId,
            Status = request.Status,
            RequestNo = request.RequestNo,
            RequestDate = request.RequestDate,
            Description = request.Description,
            ApprovalRequestId = request.ApprovalRequestId,
            CreatedAt = DateTime.UtcNow,
        };
        _db.Requests.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateRequestRequest request, CancellationToken ct = default)
    {
        var entity = await _db.Requests.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.RequestTypeId = request.RequestTypeId;
            entity.ProjectId = request.ProjectId;
            entity.RequestedByUserId = request.RequestedByUserId;
            entity.Status = request.Status;
            entity.RequestNo = request.RequestNo;
            entity.RequestDate = request.RequestDate;
            entity.Description = request.Description;
            entity.ApprovalRequestId = request.ApprovalRequestId;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.Requests.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
