using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.ChatGroupMember.Requests;
using Energy.Shared.Models.V1.Chat.ChatGroupMember.Responses;

namespace Energy.Application.Chat.ChatGroupMember.Services;

/// <summary>ChatGroupMember CRUD use-case sözleşmesi.</summary>
public interface IChatGroupMemberService
{
    /// <summary>Sayfalanmış ChatGroupMember listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ChatGroupMemberListResponse>>> GetListAsync(GetChatGroupMemberListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ChatGroupMemberDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateChatGroupMemberRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateChatGroupMemberRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
