using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Chat.ChatMessageReaction.Services;
using Energy.Shared.Models.V1.Chat.ChatMessageReaction.Requests;
using Energy.Shared.Models.V1.Chat.ChatMessageReaction.Responses;

namespace Energy.Infrastructure.Modules.Chat.ChatMessageReaction.Services;

/// <summary>ChatMessageReaction CRUD servisi (projection, pagination, soft-delete).</summary>
public class ChatMessageReactionService : IChatMessageReactionService
{
    private readonly EnergyDbContext _db;

    public ChatMessageReactionService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ChatMessageReactionListResponse>>> GetListAsync(GetChatMessageReactionListRequest request, CancellationToken ct = default)
    {
        var query = _db.ChatMessageReactions.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ChatMessageReactionListResponse
            {
                Id = e.Id,
                MessageId = e.MessageId,
                UserId = e.UserId,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ChatMessageReactionListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ChatMessageReactionListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ChatMessageReactionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ChatMessageReactions.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ChatMessageReactionDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                MessageId = e.MessageId,
                UserId = e.UserId
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ChatMessageReactionDetailResponse>.Failure("NotFound")
            : BaseResponse<ChatMessageReactionDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateChatMessageReactionRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Chat.ChatMessageReaction
        {
            Id = Guid.NewGuid(),
            MessageId = request.MessageId,
            UserId = request.UserId,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ChatMessageReactions.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateChatMessageReactionRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ChatMessageReactions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.MessageId = request.MessageId;
            entity.UserId = request.UserId;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ChatMessageReactions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
