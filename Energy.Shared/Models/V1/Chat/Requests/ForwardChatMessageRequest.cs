namespace Energy.Shared.Models.V1.Chat.Requests;

/// <summary>Var olan bir mesajı (metni + ekini) yeni bir hedefe iletir.</summary>
public sealed class ForwardChatMessageRequest
{
    /// <summary>İletilecek kaynak mesajın kimliği.</summary>
    public Guid MessageId { get; set; }

    /// <summary>Doğrudan iletim için hedef kullanıcı. Gruba iletilirken null olur.</summary>
    public Guid? RecipientId { get; set; }

    /// <summary>Hedef grup. Bir kullanıcıya iletilirken null olur.</summary>
    public Guid? GroupId { get; set; }
}
