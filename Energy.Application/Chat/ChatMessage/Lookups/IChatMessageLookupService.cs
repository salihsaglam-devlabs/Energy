using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.ChatMessage.Responses;

namespace Energy.Application.Chat.ChatMessage.Lookups;

/// <summary>ChatMessage lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IChatMessageLookupService
{
    /// <summary>ChatMessage lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ChatMessageLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
