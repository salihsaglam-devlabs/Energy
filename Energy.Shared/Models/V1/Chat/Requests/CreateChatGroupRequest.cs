namespace Energy.Shared.Models.V1.Chat.Requests;

/// <summary>Bir sohbet grubu oluşturur ve verilen kullanıcıları isteğe bağlı olarak davet eder.</summary>
public sealed class CreateChatGroupRequest
{
    /// <summary>Oluşturulacak grubun adı.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Oluşturma sırasında davet edilecek kullanıcılar (her biri beklemede bir davet alır).</summary>
    public IReadOnlyList<Guid> MemberUserIds { get; set; } = [];
}
