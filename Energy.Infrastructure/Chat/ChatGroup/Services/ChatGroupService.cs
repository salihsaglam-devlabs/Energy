using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Chat.ChatGroup.Services;
using Energy.Shared.Models.V1.Chat.ChatGroup.Requests;
using Energy.Shared.Models.V1.Chat.ChatGroup.Responses;

namespace Energy.Infrastructure.Chat.ChatGroup.Services;

/// <summary>ChatGroup CRUD servisi (projection, pagination, soft-delete).</summary>
public class ChatGroupService : IChatGroupService
{
    private readonly AppDbContext _db;

    public ChatGroupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ChatGroupListResponse>>> GetListAsync(GetChatGroupListRequest request, CancellationToken ct = default)
    {
        var query = _db.ChatGroups.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ChatGroupListResponse
            {
                Id = e.Id,
                OwnerId = e.OwnerId,
                Name = e.Name,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ChatGroupListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ChatGroupListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ChatGroupDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ChatGroups.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ChatGroupDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                OwnerId = e.OwnerId,
                Name = e.Name
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ChatGroupDetailResponse>.Failure("NotFound")
            : BaseResponse<ChatGroupDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateChatGroupRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Chat.ChatGroup
        {
            Id = Guid.NewGuid(),
            OwnerId = request.OwnerId,
            Name = request.Name,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ChatGroups.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateChatGroupRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ChatGroups.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.OwnerId = request.OwnerId;
            entity.Name = request.Name;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ChatGroups.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
