using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.ChatMessageReaction.Requests;
using Energy.Shared.Models.V1.Chat.ChatMessageReaction.Responses;

namespace Energy.Application.Modules.Chat.ChatMessageReaction.Services;

/// <summary>ChatMessageReaction CRUD use-case sözleşmesi.</summary>
public interface IChatMessageReactionService
{
    /// <summary>Sayfalanmış ChatMessageReaction listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ChatMessageReactionListResponse>>> GetListAsync(GetChatMessageReactionListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ChatMessageReactionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateChatMessageReactionRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateChatMessageReactionRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
