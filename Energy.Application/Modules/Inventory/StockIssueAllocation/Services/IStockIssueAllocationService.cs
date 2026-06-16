using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockIssueAllocation.Requests;
using Energy.Shared.Models.V1.Inventory.StockIssueAllocation.Responses;

namespace Energy.Application.Modules.Inventory.StockIssueAllocation.Services;

/// <summary>StockIssueAllocation CRUD use-case sözleşmesi.</summary>
public interface IStockIssueAllocationService
{
    /// <summary>Sayfalanmış StockIssueAllocation listesi.</summary>
    Task<BaseResponse<PaginatedResponse<StockIssueAllocationListResponse>>> GetListAsync(GetStockIssueAllocationListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<StockIssueAllocationDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateStockIssueAllocationRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateStockIssueAllocationRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
