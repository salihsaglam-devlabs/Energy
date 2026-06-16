using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.ChatGroupMember.Responses;

namespace Energy.Application.Chat.ChatGroupMember.Lookups;

/// <summary>ChatGroupMember lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IChatGroupMemberLookupService
{
    /// <summary>ChatGroupMember lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ChatGroupMemberLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
