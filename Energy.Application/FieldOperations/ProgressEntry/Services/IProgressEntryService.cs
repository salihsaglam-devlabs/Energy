using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.ProgressEntry.Requests;
using Energy.Shared.Models.V1.FieldOperations.ProgressEntry.Responses;

namespace Energy.Application.FieldOperations.ProgressEntry.Services;

/// <summary>ProgressEntry CRUD use-case sözleşmesi.</summary>
public interface IProgressEntryService
{
    /// <summary>Sayfalanmış ProgressEntry listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ProgressEntryListResponse>>> GetListAsync(GetProgressEntryListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ProgressEntryDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateProgressEntryRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateProgressEntryRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
