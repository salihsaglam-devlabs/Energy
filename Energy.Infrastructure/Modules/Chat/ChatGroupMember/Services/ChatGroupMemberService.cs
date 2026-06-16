using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Chat.ChatGroupMember.Services;
using Energy.Shared.Models.V1.Chat.ChatGroupMember.Requests;
using Energy.Shared.Models.V1.Chat.ChatGroupMember.Responses;

namespace Energy.Infrastructure.Modules.Chat.ChatGroupMember.Services;

/// <summary>ChatGroupMember CRUD servisi (projection, pagination, soft-delete).</summary>
public class ChatGroupMemberService : IChatGroupMemberService
{
    private readonly AppDbContext _db;

    public ChatGroupMemberService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ChatGroupMemberListResponse>>> GetListAsync(GetChatGroupMemberListRequest request, CancellationToken ct = default)
    {
        var query = _db.ChatGroupMembers.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ChatGroupMemberListResponse
            {
                Id = e.Id,
                GroupId = e.GroupId,
                UserId = e.UserId,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ChatGroupMemberListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ChatGroupMemberListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ChatGroupMemberDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ChatGroupMembers.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ChatGroupMemberDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                GroupId = e.GroupId,
                UserId = e.UserId
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ChatGroupMemberDetailResponse>.Failure("NotFound")
            : BaseResponse<ChatGroupMemberDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateChatGroupMemberRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Chat.ChatGroupMember
        {
            Id = Guid.NewGuid(),
            GroupId = request.GroupId,
            UserId = request.UserId,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ChatGroupMembers.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateChatGroupMemberRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ChatGroupMembers.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.GroupId = request.GroupId;
            entity.UserId = request.UserId;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ChatGroupMembers.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
