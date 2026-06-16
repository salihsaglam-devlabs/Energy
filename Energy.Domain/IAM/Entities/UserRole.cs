namespace Energy.Domain.IAM;

/// <summary>
/// Kullanıcı–rol bağlantı (join) tablosu. Kalıcı (hard) silme uygulanır: satırın
/// kaldırılması rol atamasını geri alır.
/// </summary>
public class UserRole
{
    /// <summary>Rolün atandığı kullanıcının kimliği.</summary>
    public Guid UserId { get; set; }

    /// <summary>Kullanıcıya atanan rolün kimliği.</summary>
    public Guid RoleId { get; set; }
}
