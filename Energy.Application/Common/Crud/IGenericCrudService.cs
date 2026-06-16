using Energy.Domain.Common;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;

namespace Energy.Application.Common.Crud;

/// <summary>
/// Kurumsal modüllerin varlıkları için ortak, tip güvenli CRUD sözleşmesi.
/// Sayfalama, tekil okuma, oluşturma, güncelleme ve (yumuşak) silme sağlar.
/// Yetkilendirme API katmanındaki uç nokta-permission eşlemesiyle uygulanır.
/// </summary>
/// <typeparam name="TEntity">Yönetilen <see cref="AuditableEntity"/> türü.</typeparam>
public interface IGenericCrudService<TEntity>
    where TEntity : AuditableEntity
{
    /// <summary>Soft-delete uygulanmış kayıtları sayfalı döndürür.</summary>
    Task<PaginatedResponse<TEntity>> GetAllAsync(PaginatedRequest request, CancellationToken ct = default);

    /// <summary>Tek bir kaydı kimliğine göre döndürür (yoksa null).</summary>
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni bir kayıt oluşturur ve oluşturulan varlığı döndürür.</summary>
    Task<TEntity> CreateAsync(TEntity entity, CancellationToken ct = default);

    /// <summary>Var olan bir kaydı günceller; kayıt yoksa null döndürür.</summary>
    Task<TEntity?> UpdateAsync(Guid id, TEntity entity, CancellationToken ct = default);

    /// <summary>Kaydı yumuşak siler; başarılıysa true döndürür.</summary>
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}

