using System.Text.Json;
using Energy.Shared.Models.V1.Common.Responses;

namespace Energy.Web.Clients.Enterprise;

/// <summary>
/// Kurumsal modüllerin generic CRUD API uç noktalarına (<c>/api/v1/{module}</c>) erişen
/// tip-bağımsız istemci. Veri şekli modüle göre değiştiğinden gövde/yanıt JSON olarak
/// (System.Text.Json <see cref="JsonElement"/>) taşınır.
/// </summary>
public interface IEnterpriseApiClient
{
    /// <summary>Modülün sayfalı listesini getirir.</summary>
    Task<BaseResponse<PaginatedResponse<JsonElement>>> ListAsync(
        string module, int pageNumber, int pageSize, string? search, CancellationToken ct = default);

    /// <summary>Tekil kaydı getirir.</summary>
    Task<BaseResponse<JsonElement>> GetByIdAsync(string module, Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur.</summary>
    Task<BaseResponse<JsonElement>> CreateAsync(string module, JsonElement body, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<JsonElement>> UpdateAsync(string module, Guid id, JsonElement body, CancellationToken ct = default);

    /// <summary>Kaydı (yumuşak) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(string module, Guid id, CancellationToken ct = default);

    /// <summary>
    /// Generic bir iş kuralı eylemi (POST) yürütür. <paramref name="apiRelativePath"/>
    /// "api/v1" segmentinden sonraki yoldur (ör. <c>inventory-actions/reverse/{id}</c>).
    /// İsteğe bağlı JSON gövdesi (ör. <c>{ note }</c>) iletilir.
    /// </summary>
    Task<BaseResponse<JsonElement>> PostActionAsync(string apiRelativePath, JsonElement? body, CancellationToken ct = default);

    /// <summary>Generic bir okuma eylemi (GET) yürütür (ör. öznitelik doğrulama).</summary>
    Task<BaseResponse<JsonElement>> GetActionAsync(string apiRelativePath, CancellationToken ct = default);

    /// <summary>
    /// Ana-detay ekranlarının alt-koleksiyonunu (satırlarını) sayfalı getirir.
    /// <paramref name="detailKey"/> <c>/api/v1/details/{detailKey}</c> rota segmentidir
    /// (ör. <c>purchase-order-lines</c>); <paramref name="parentId"/> başlık kaydının kimliğidir.
    /// </summary>
    Task<BaseResponse<PaginatedResponse<JsonElement>>> ListChildrenAsync(
        string detailKey, Guid parentId, int pageNumber, int pageSize, CancellationToken ct = default);

    /// <summary>Verilen başlığa bağlı yeni bir alt satır oluşturur.</summary>
    Task<BaseResponse<JsonElement>> CreateChildAsync(string detailKey, Guid parentId, JsonElement body, CancellationToken ct = default);

    /// <summary>Var olan bir alt satırı günceller (başlık bağı API tarafında korunur).</summary>
    Task<BaseResponse<JsonElement>> UpdateChildAsync(string detailKey, Guid id, JsonElement body, CancellationToken ct = default);

    /// <summary>Bir alt satırı (yumuşak) siler.</summary>
    Task<BaseResponse<bool>> DeleteChildAsync(string detailKey, Guid id, CancellationToken ct = default);
}

