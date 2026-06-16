using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockReservation.Requests;
using Energy.Shared.Models.V1.Inventory.StockReservation.Responses;

namespace Energy.Application.Modules.Inventory.StockReservation.Services;

/// <summary>StockReservation CRUD use-case sözleşmesi.</summary>
public interface IStockReservationService
{
    /// <summary>Sayfalanmış StockReservation listesi.</summary>
    Task<BaseResponse<PaginatedResponse<StockReservationListResponse>>> GetListAsync(GetStockReservationListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<StockReservationDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateStockReservationRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateStockReservationRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
