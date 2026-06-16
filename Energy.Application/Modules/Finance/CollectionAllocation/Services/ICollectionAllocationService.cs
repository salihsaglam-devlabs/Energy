using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.CollectionAllocation.Requests;
using Energy.Shared.Models.V1.Finance.CollectionAllocation.Responses;

namespace Energy.Application.Modules.Finance.CollectionAllocation.Services;

/// <summary>CollectionAllocation CRUD use-case sözleşmesi.</summary>
public interface ICollectionAllocationService
{
    /// <summary>Sayfalanmış CollectionAllocation listesi.</summary>
    Task<BaseResponse<PaginatedResponse<CollectionAllocationListResponse>>> GetListAsync(GetCollectionAllocationListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<CollectionAllocationDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateCollectionAllocationRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateCollectionAllocationRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
