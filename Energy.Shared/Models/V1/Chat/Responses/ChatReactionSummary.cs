namespace Energy.Shared.Models.V1.Chat.Responses;

/// <summary>Bir mesaj üzerindeki tek bir emoji için toplulaştırılmış tepki bilgisi.</summary>
public sealed class ChatReactionSummary
{
    /// <summary>Tepki emojisi.</summary>
    public string Emoji { get; set; } = string.Empty;

    /// <summary>Bu emojiyle tepki veren kişi sayısı.</summary>
    public int Count { get; set; }

    /// <summary>Geçerli kullanıcı bu emojiyle tepki verenler arasındaysa true.</summary>
    public bool Reacted { get; set; }
}
