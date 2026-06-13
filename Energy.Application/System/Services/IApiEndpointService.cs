using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;

namespace Energy.Application.System.Services;

/// <summary>API endpoint kataloğunun yönetimi ve istek-yetki çözümleme servisi.</summary>
public interface IApiEndpointService
{
    /// <summary>API endpoint'lerini sayfalı olarak döndürür.</summary>
    Task<PaginatedResponse<ApiEndpointResponse>> GetAllAsync(PaginatedRequest request, CancellationToken cancellationToken = default);

    /// <summary>Belirtilen kimliğe sahip endpoint'i döndürür; yoksa null.</summary>
    Task<ApiEndpointResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>Yeni bir API endpoint kaydı oluşturur.</summary>
    Task<ApiEndpointResponse> CreateAsync(CreateApiEndpointRequest request, CancellationToken cancellationToken = default);

    /// <summary>Mevcut bir API endpoint kaydını günceller.</summary>
    Task<ApiEndpointResponse> UpdateAsync(Guid id, UpdateApiEndpointRequest request, CancellationToken cancellationToken = default);

    /// <summary>Bir API endpoint kaydını siler; başarılıysa true döner.</summary>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>Verilen isteğe (rota şablonunu dikkate alarak) uyan endpoint'i bulur.</summary>
    Task<ApiEndpointResponse?> ResolveAsync(string httpMethod, string path, CancellationToken cancellationToken = default);

    /// <summary>Bellek içi endpoint arama önbelleğini geçersiz kılar.</summary>
    void InvalidateCache();
}
