namespace Energy.Shared.Models.V1.Chat.Requests;

/// <summary>Geçerli kullanıcının bir mesaj üzerindeki emoji tepkisini açıp kapatır.</summary>
public sealed class ReactChatMessageRequest
{
    /// <summary>Tepki emojisi. Aynı emoji tekrar gönderildiğinde tepki kaldırılır.</summary>
    public string Emoji { get; set; } = string.Empty;
}
