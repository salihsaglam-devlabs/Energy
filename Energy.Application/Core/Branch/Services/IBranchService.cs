using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Branch.Requests;
using Energy.Shared.Models.V1.Core.Branch.Responses;

namespace Energy.Application.Core.Branch.Services;

/// <summary>Branch CRUD use-case sözleşmesi.</summary>
public interface IBranchService
{
    /// <summary>Sayfalanmış Branch listesi.</summary>
    Task<BaseResponse<PaginatedResponse<BranchListResponse>>> GetListAsync(GetBranchListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<BranchDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateBranchRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateBranchRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
