using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.ChatMessageReaction.Responses;

namespace Energy.Application.Chat.ChatMessageReaction.Lookups;

/// <summary>ChatMessageReaction lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IChatMessageReactionLookupService
{
    /// <summary>ChatMessageReaction lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ChatMessageReactionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
