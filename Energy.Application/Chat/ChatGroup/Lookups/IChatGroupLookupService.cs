using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.ChatGroup.Responses;

namespace Energy.Application.Chat.ChatGroup.Lookups;

/// <summary>ChatGroup lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IChatGroupLookupService
{
    /// <summary>ChatGroup lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ChatGroupLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
