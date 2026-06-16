using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeDefinition.Responses;

namespace Energy.Application.Modules.Catalog.MaterialAttributeDefinition.Lookups;

/// <summary>MaterialAttributeDefinition lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IMaterialAttributeDefinitionLookupService
{
    /// <summary>MaterialAttributeDefinition lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<MaterialAttributeDefinitionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
