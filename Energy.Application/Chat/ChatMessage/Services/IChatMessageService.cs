using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.ChatMessage.Requests;
using Energy.Shared.Models.V1.Chat.ChatMessage.Responses;

namespace Energy.Application.Chat.ChatMessage.Services;

/// <summary>ChatMessage CRUD use-case sözleşmesi.</summary>
public interface IChatMessageService
{
    /// <summary>Sayfalanmış ChatMessage listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ChatMessageListResponse>>> GetListAsync(GetChatMessageListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ChatMessageDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateChatMessageRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateChatMessageRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
