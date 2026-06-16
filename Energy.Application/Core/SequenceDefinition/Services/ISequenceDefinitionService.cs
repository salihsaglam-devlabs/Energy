using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.SequenceDefinition.Requests;
using Energy.Shared.Models.V1.Core.SequenceDefinition.Responses;

namespace Energy.Application.Core.SequenceDefinition.Services;

/// <summary>SequenceDefinition CRUD use-case sözleşmesi.</summary>
public interface ISequenceDefinitionService
{
    /// <summary>Sayfalanmış SequenceDefinition listesi.</summary>
    Task<BaseResponse<PaginatedResponse<SequenceDefinitionListResponse>>> GetListAsync(GetSequenceDefinitionListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<SequenceDefinitionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateSequenceDefinitionRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateSequenceDefinitionRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
