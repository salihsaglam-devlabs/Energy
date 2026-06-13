using Energy.Domain.Common;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;

namespace Energy.Application.Common.Crud;

/// <summary>
/// Ana-detay (master-detail) ekranları için ortak alt-koleksiyon sorgu sözleşmesi.
/// Bir başlık kaydının (ör. Sözleşme) kimliği verildiğinde, o başlığa yabancı anahtarla
/// bağlı satır varlıklarını (ör. SözleşmeKalemleri) soft-delete farkında ve sayfalı olarak
/// döndürür. Yetkilendirme, API katmanındaki uç nokta-permission eşlemesiyle uygulanır;
/// her alt-koleksiyon kendi ana modülünün <c>ReadAll</c> yetkisini gerektirir.
/// </summary>
public interface IModuleDetailQueryService
{
    /// <summary>
    /// <paramref name="foreignKeyProperty"/> alanı <paramref name="parentId"/> değerine eşit
    /// olan <typeparamref name="TChild"/> satırlarını sayfalı döndürür.
    /// </summary>
    /// <typeparam name="TChild">Başlığa bağlı satır <see cref="AuditableEntity"/> türü.</typeparam>
    /// <param name="foreignKeyProperty">Satırdaki, başlığı işaret eden yabancı anahtar alanının adı.</param>
    /// <param name="parentId">Başlık kaydının kimliği.</param>
    Task<PaginatedResponse<TChild>> GetChildrenAsync<TChild>(
        string foreignKeyProperty,
        Guid parentId,
        PaginatedRequest request,
        CancellationToken ct = default)
        where TChild : AuditableEntity;
}

