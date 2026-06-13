using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;

namespace Energy.Application.Identity.Services;

/// <summary>Rollerin yönetimi (listeleme, oluşturma, güncelleme, silme ve yetki atama) servisi.</summary>
public interface IRoleService
{
    /// <summary>Rolleri sayfalı olarak (arama/sıralama ile) döndürür.</summary>
    Task<PaginatedResponse<RoleSummaryResponse>> GetAllAsync(PaginatedRequest request, CancellationToken cancellationToken = default);

    /// <summary>Belirtilen kimliğe sahip rolün ayrıntılarını döndürür; yoksa null.</summary>
    Task<RoleDetailResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>Yeni bir rol oluşturur ve ayrıntılarını döndürür.</summary>
    Task<RoleDetailResponse> CreateAsync(CreateRoleRequest request, CancellationToken cancellationToken = default);

    /// <summary>Mevcut bir rolü günceller ve güncel ayrıntılarını döndürür.</summary>
    Task<RoleDetailResponse> UpdateAsync(Guid id, UpdateRoleRequest request, CancellationToken cancellationToken = default);

    /// <summary>Bir rolü siler; başarılıysa true döner.</summary>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>Bir rolün yetki kümesini verilen liste ile değiştirir.</summary>
    Task<RoleDetailResponse> SetPermissionsAsync(Guid id, SetRolePermissionsRequest request, CancellationToken cancellationToken = default);
}
