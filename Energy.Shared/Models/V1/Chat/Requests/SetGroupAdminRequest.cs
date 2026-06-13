namespace Energy.Shared.Models.V1.Chat.Requests;

/// <summary>Bir üyeyi grup yöneticisi yapar veya normal üye konumuna geri indirir.</summary>
public sealed class SetGroupAdminRequest
{
    /// <summary>Yönetici yetkisi vermek için true, kaldırmak için false.</summary>
    public bool IsAdmin { get; set; }
}
