using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Chat.ChatMessage.Services;
using Energy.Shared.Models.V1.Chat.ChatMessage.Requests;
using Energy.Shared.Models.V1.Chat.ChatMessage.Responses;

namespace Energy.Infrastructure.Chat.ChatMessage.Services;

/// <summary>ChatMessage CRUD servisi (projection, pagination, soft-delete).</summary>
public class ChatMessageService : IChatMessageService
{
    private readonly AppDbContext _db;

    public ChatMessageService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ChatMessageListResponse>>> GetListAsync(GetChatMessageListRequest request, CancellationToken ct = default)
    {
        var query = _db.ChatMessages.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ChatMessageListResponse
            {
                Id = e.Id,
                SenderId = e.SenderId,
                RecipientId = e.RecipientId,
                GroupId = e.GroupId,
                ReplyToMessageId = e.ReplyToMessageId,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ChatMessageListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ChatMessageListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ChatMessageDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ChatMessages.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ChatMessageDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                SenderId = e.SenderId,
                RecipientId = e.RecipientId,
                GroupId = e.GroupId,
                ReplyToMessageId = e.ReplyToMessageId
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ChatMessageDetailResponse>.Failure("NotFound")
            : BaseResponse<ChatMessageDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateChatMessageRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Chat.ChatMessage
        {
            Id = Guid.NewGuid(),
            SenderId = request.SenderId,
            RecipientId = request.RecipientId,
            GroupId = request.GroupId,
            ReplyToMessageId = request.ReplyToMessageId,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ChatMessages.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateChatMessageRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ChatMessages.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.SenderId = request.SenderId;
            entity.RecipientId = request.RecipientId;
            entity.GroupId = request.GroupId;
            entity.ReplyToMessageId = request.ReplyToMessageId;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ChatMessages.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
