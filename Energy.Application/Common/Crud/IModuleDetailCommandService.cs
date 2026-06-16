using Energy.Domain.Common;

namespace Energy.Application.Common.Crud;

/// <summary>
/// Ana-detay (master-detail) ekranlarının alt-koleksiyonları için ortak yazma sözleşmesi.
/// Oluşturma sırasında satırın yabancı anahtarı her zaman başlık kaydına zorlanır; güncelleme
/// sırasında başlık bağı (yabancı anahtar) ve oluşturma denetimi korunur — yani bir satır,
/// güncelleme yoluyla başka bir başlığa taşınamaz. Silme yumuşak silmedir (interceptor).
/// Yetkilendirme, API katmanındaki uç nokta-permission eşlemesiyle uygulanır; her alt-koleksiyon
/// kendi ana modülünün <c>Create/Update/Delete</c> yetkisini gerektirir.
/// </summary>
public interface IModuleDetailCommandService
{
    /// <summary>
    /// Yeni bir alt satır oluşturur; <paramref name="foreignKeyProperty"/> alanı her durumda
    /// <paramref name="parentId"/> değerine ayarlanır (gövdeden gelen değer dikkate alınmaz).
    /// </summary>
    Task<TChild> CreateChildAsync<TChild>(
        string foreignKeyProperty,
        Guid parentId,
        TChild entity,
        CancellationToken ct = default)
        where TChild : AuditableEntity;

    /// <summary>
    /// Var olan bir alt satırı günceller; başlık bağı (<paramref name="foreignKeyProperty"/>)
    /// ve oluşturma denetimi korunur. Kayıt yoksa <c>null</c> döner.
    /// </summary>
    Task<TChild?> UpdateChildAsync<TChild>(
        string foreignKeyProperty,
        Guid id,
        TChild entity,
        CancellationToken ct = default)
        where TChild : AuditableEntity;

    /// <summary>Bir alt satırı yumuşak siler; başarılıysa <c>true</c> döner.</summary>
    Task<bool> DeleteChildAsync<TChild>(Guid id, CancellationToken ct = default)
        where TChild : AuditableEntity;
}

