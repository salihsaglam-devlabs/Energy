using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;

namespace Energy.Application.System.Services;

/// <summary>Menü ağacının yönetimi ve kullanıcıya özel görünür menü ağacını üretme servisi.</summary>
public interface IMenuService
{
    /// <summary>Menüleri sayfalı olarak döndürür.</summary>
    Task<PaginatedResponse<MenuResponse>> GetAllAsync(PaginatedRequest request, CancellationToken cancellationToken = default);

    /// <summary>Belirtilen kimliğe sahip menüyü döndürür; yoksa null.</summary>
    Task<MenuResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>Yeni bir menü düğümü oluşturur.</summary>
    Task<MenuResponse> CreateAsync(CreateMenuRequest request, CancellationToken cancellationToken = default);

    /// <summary>Mevcut bir menü düğümünü günceller.</summary>
    Task<MenuResponse> UpdateAsync(Guid id, UpdateMenuRequest request, CancellationToken cancellationToken = default);

    /// <summary>Bir menü düğümünü siler; başarılıysa true döner.</summary>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>Belirtilen kullanıcı için görünür menü ağacını döndürür (null ise anonim).</summary>
    Task<IReadOnlyList<MenuTreeNodeResponse>> GetTreeForUserAsync(Guid? userId, CancellationToken cancellationToken = default);
}
