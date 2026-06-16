using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Currency.Requests;
using Energy.Shared.Models.V1.Core.Currency.Responses;

namespace Energy.Application.Modules.Core.Currency.Services;

/// <summary>Currency CRUD use-case sözleşmesi.</summary>
public interface ICurrencyService
{
    /// <summary>Sayfalanmış Currency listesi.</summary>
    Task<BaseResponse<PaginatedResponse<CurrencyListResponse>>> GetListAsync(GetCurrencyListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<CurrencyDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateCurrencyRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateCurrencyRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
