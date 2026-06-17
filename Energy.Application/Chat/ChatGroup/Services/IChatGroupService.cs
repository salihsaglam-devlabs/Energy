using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.ChatGroup.Requests;
using Energy.Shared.Models.V1.Chat.ChatGroup.Responses;

namespace Energy.Application.Chat.ChatGroup.Services;

/// <summary>ChatGroup CRUD use-case sözleşmesi.</summary>
public interface IChatGroupService
{
    /// <summary>Sayfalanmış ChatGroup listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ChatGroupListResponse>>> GetListAsync(GetChatGroupListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ChatGroupDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateChatGroupRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateChatGroupRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
