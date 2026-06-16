using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Collection.Requests;
using Energy.Shared.Models.V1.Finance.Collection.Responses;

namespace Energy.Application.Modules.Finance.Collection.Services;

/// <summary>Collection CRUD use-case sözleşmesi.</summary>
public interface ICollectionService
{
    /// <summary>Sayfalanmış Collection listesi.</summary>
    Task<BaseResponse<PaginatedResponse<CollectionListResponse>>> GetListAsync(GetCollectionListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<CollectionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateCollectionRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateCollectionRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
