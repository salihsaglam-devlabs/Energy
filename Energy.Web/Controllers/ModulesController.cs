using System.Text.Json;
using System.Text.Json.Nodes;
using Energy.Web.Clients.Enterprise;
using Energy.Web.Common.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Energy.Web.Controllers;

/// <summary>
/// Kurumsal modüllerin tek, generic DevExtreme CRUD ekranı. Rota: <c>/m/{module}</c>.
/// Tüm CRUD çağrılarını generic API uç noktalarına (<c>/api/v1/{module}</c>) iletir.
/// Yetkilendirme API tarafında uç nokta-permission eşlemesiyle uygulanır; yetkisiz
/// çağrılar <see cref="ApiExceptionFilter"/> ile login/erişim-reddi ekranına yönlenir.
/// </summary>
[Authorize]
[Route("m")]
public sealed class ModulesController : Controller
{
    /// <summary>İzin verilen modüller: rota segmenti → (permission modülü, başlık anahtarı).</summary>
    private static readonly IReadOnlyDictionary<string, (string PermModule, string TitleKey)> Modules =
        new Dictionary<string, (string, string)>(StringComparer.OrdinalIgnoreCase)
        {
            ["core"] = ("Core", "Menus.CoreData"),
            ["organization"] = ("Organization", "Menus.Organization"),
            ["business-partners"] = ("BusinessPartners", "Menus.BusinessPartners"),
            ["projects"] = ("Projects", "Menus.Projects"),
            ["catalog"] = ("Catalog", "Menus.Catalog"),
            ["inventory"] = ("Inventory", "Menus.Inventory"),
            ["requests"] = ("Requests", "Menus.Requests"),
            ["procurement"] = ("Procurement", "Menus.Procurement"),
            ["operations"] = ("Operations", "Menus.Operations"),
            ["field-operations"] = ("FieldOperations", "Menus.FieldOperations"),
            ["hr"] = ("HR", "Menus.HR"),
            ["assets"] = ("Assets", "Menus.Assets"),
            ["finance"] = ("Finance", "Menus.Finance"),
            ["budget"] = ("Budget", "Menus.Budget"),
            ["contracts"] = ("Contracts", "Menus.Contracts"),
            ["progress-payments"] = ("ProgressPayments", "Menus.ProgressPayments"),
            ["documents"] = ("Documents", "Menus.Documents"),
            ["workflow"] = ("Workflow", "Menus.Workflow"),
            ["notifications"] = ("Notifications", "Menus.Notifications"),
            ["reporting"] = ("Reporting", "Menus.Reporting"),
        };

    private readonly IEnterpriseApiClient _api;

    public ModulesController(IEnterpriseApiClient api) => _api = api;

    /// <summary>
    /// İzin verilen ana-detay alt-koleksiyonları: rota segmenti (modül) → izin verilen
    /// detay anahtarları (<c>/api/v1/details/{detailKey}</c>). Bu beyaz liste, proxy'nin
    /// yalnızca bilinen alt-koleksiyon uç noktalarına çağrı yapmasını sağlar; yetkilendirme
    /// API tarafında ana modülün ReadAll yetkisiyle uç nokta-permission eşlemesiyle uygulanır.
    /// </summary>
    private static readonly IReadOnlyDictionary<string, string[]> ModuleDetails =
        new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            ["requests"] = new[] { "request-lines" },
            ["procurement"] = new[] { "purchase-order-lines" },
            ["operations"] = new[]
            {
                "work-order-assignments", "work-order-material-plans",
                "work-order-checklists", "work-order-status-histories"
            },
            ["field-operations"] = new[]
            {
                "daily-site-report-workers", "daily-site-report-equipments", "daily-site-report-materials"
            },
            ["hr"] = new[] { "timesheet-lines" },
            ["assets"] = new[] { "equipment-assignments", "equipment-maintenances" },
            ["finance"] = new[] { "financial-transaction-lines" },
            ["budget"] = new[] { "budget-lines" },
            ["contracts"] = new[] { "contract-lines", "contract-parties", "contract-amendments" },
            ["progress-payments"] = new[] { "progress-payment-lines", "progress-payment-deductions" },
            ["catalog"] = new[] { "material-attribute-values", "material-unit-conversions" },
            ["inventory"] = new[] { "warehouse-locations" },
        };

    /// <summary>
    /// Salt-okunur (yazma sunulmayan) alt-koleksiyonlar. Denetim/iz niteliğindeki bu
    /// koleksiyonlar yalnızca <c>GET</c> proxy'sine izin verir; create/update/delete reddedilir.
    /// </summary>
    private static readonly HashSet<string> ReadOnlyDetailKeys =
        new(StringComparer.OrdinalIgnoreCase) { "work-order-status-histories" };

    /// <summary>Verilen modül için detay anahtarının okunabilir olup olmadığını doğrular.</summary>
    private static bool CanReadDetail(string module, string detailKey)
        => ModuleDetails.TryGetValue(module, out var allowed) &&
           Array.IndexOf(allowed, detailKey.ToLowerInvariant()) >= 0;

    /// <summary>Verilen detay anahtarının yazılabilir (salt-okunur olmayan) olup olmadığını doğrular.</summary>
    private static bool CanWriteDetail(string module, string detailKey)
        => CanReadDetail(module, detailKey) && !ReadOnlyDetailKeys.Contains(detailKey);

    /// <summary>
    /// İzin verilen iş kuralı satır eylemleri: (modül, eylem anahtarı) → (HTTP metodu,
    /// API yol şablonu). Şablondaki <c>{id}</c> seçili satırın kimliğiyle değiştirilir.
    /// Bu beyaz liste, proxy'nin yalnızca bilinen action uç noktalarına çağrı yapmasını
    /// sağlar; yetkilendirme API tarafında uç nokta-permission eşlemesiyle uygulanır.
    /// </summary>
    private static readonly IReadOnlyDictionary<(string Module, string Action), (string Method, string PathTemplate)> RowActions =
        new Dictionary<(string, string), (string, string)>()
        {
            // Workflow (onay) eylemleri
            [("workflow", "approve")] = ("POST", "workflow-actions/{id}/approve"),
            [("workflow", "reject")]  = ("POST", "workflow-actions/{id}/reject"),
            [("workflow", "return")]  = ("POST", "workflow-actions/{id}/return"),
            [("workflow", "cancel")]  = ("POST", "workflow-actions/{id}/cancel"),
            // Inventory
            [("inventory", "reverse")] = ("POST", "inventory-actions/reverse/{id}"),
            // Procurement
            [("procurement", "receive")] = ("POST", "procurement-actions/receive/{id}"),
            // Operations
            [("operations", "close")]  = ("POST", "operations-actions/{id}/close"),
            [("operations", "reopen")] = ("POST", "operations-actions/{id}/reopen"),
            // Catalog
            [("catalog", "activate")] = ("POST", "catalog-actions/{id}/activate"),
            [("catalog", "validate")] = ("GET", "catalog-actions/{id}/validate"),
        };

    [HttpGet("{module}")]
    public IActionResult Index(string module)
    {
        if (!Modules.TryGetValue(module, out var meta))
        {
            return NotFound();
        }

        ViewData["Module"] = module.ToLowerInvariant();
        ViewData["PermModule"] = meta.PermModule;
        ViewData["TitleKey"] = meta.TitleKey;
        return View();
    }

    [HttpGet("{module}/list")]
    public async Task<IActionResult> List(string module, int skip = 0, int take = 20, string? searchValue = null, CancellationToken ct = default)
    {
        if (!Modules.ContainsKey(module))
        {
            return NotFound();
        }

        var pageNumber = (take <= 0 ? 1 : skip / take) + 1;
        var envelope = await _api.ListAsync(module, pageNumber, take <= 0 ? 20 : take,
            string.IsNullOrWhiteSpace(searchValue) ? null : searchValue, ct);

        var page = envelope.Data;
        var items = page?.Items ?? Array.Empty<JsonElement>();
        return Json(new { data = items, totalCount = page?.TotalCount ?? 0 });
    }

    /// <summary>
    /// Bir başlık kaydının (<paramref name="parentId"/>) alt-koleksiyonunu (satırlarını)
    /// sayfalı döndürür; ana-detay (master-detail) grid'lerini besler. Detay anahtarı
    /// modülün <see cref="ModuleDetails"/> beyaz listesinde değilse 404 döner.
    /// </summary>
    [HttpGet("{module}/details/{detailKey}")]
    public async Task<IActionResult> Detail(
        string module, string detailKey, Guid parentId, int skip = 0, int take = 20, CancellationToken ct = default)
    {
        if (!CanReadDetail(module, detailKey))
        {
            return NotFound();
        }

        if (parentId == Guid.Empty)
        {
            return Json(new { data = Array.Empty<JsonElement>(), totalCount = 0 });
        }

        var pageNumber = (take <= 0 ? 1 : skip / take) + 1;
        var envelope = await _api.ListChildrenAsync(detailKey, parentId, pageNumber, take <= 0 ? 20 : take, ct);

        var page = envelope.Data;
        var items = page?.Items ?? Array.Empty<JsonElement>();
        return Json(new { data = items, totalCount = page?.TotalCount ?? 0 });
    }

    /// <summary>Bir başlığa bağlı yeni alt satır oluşturur (detay grid satır ekleme).</summary>
    [HttpPost("{module}/details/{detailKey}")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> DetailCreate(
        string module, string detailKey, Guid parentId, [FromBody] JsonElement body, CancellationToken ct)
    {
        if (!CanWriteDetail(module, detailKey) || parentId == Guid.Empty)
        {
            return NotFound();
        }

        var envelope = await _api.CreateChildAsync(detailKey, parentId, body, ct);
        return Json(envelope);
    }

    /// <summary>Var olan bir alt satırı günceller (detay grid satır düzenleme).</summary>
    [HttpPut("{module}/details/{detailKey}/{id:guid}")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> DetailUpdate(
        string module, string detailKey, Guid id, [FromBody] JsonElement body, CancellationToken ct)
    {
        if (!CanWriteDetail(module, detailKey))
        {
            return NotFound();
        }

        var envelope = await _api.UpdateChildAsync(detailKey, id, body, ct);
        return Json(envelope);
    }

    /// <summary>Bir alt satırı (yumuşak) siler (detay grid satır silme).</summary>
    [HttpDelete("{module}/details/{detailKey}/{id:guid}")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> DetailDelete(
        string module, string detailKey, Guid id, CancellationToken ct)
    {
        if (!CanWriteDetail(module, detailKey))
        {
            return NotFound();
        }

        var envelope = await _api.DeleteChildAsync(detailKey, id, ct);
        return Json(envelope);
    }
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Create(string module, [FromBody] JsonElement body, CancellationToken ct)
    {
        if (!Modules.ContainsKey(module))
        {
            return NotFound();
        }

        var envelope = await _api.CreateAsync(module, body, ct);
        return Json(envelope);
    }

    [HttpPut("{module}/{id:guid}")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Update(string module, Guid id, [FromBody] JsonElement body, CancellationToken ct)
    {
        if (!Modules.ContainsKey(module))
        {
            return NotFound();
        }

        // DevExtreme yalnızca değişen alanları gönderir; veri kaybını önlemek için
        // mevcut kaydı çekip değişiklikleri birleştirerek tam nesneyi PUT ederiz.
        var current = await _api.GetByIdAsync(module, id, ct);
        var merged = current.Data.ValueKind == JsonValueKind.Object
            ? JsonNode.Parse(current.Data.GetRawText()) as JsonObject ?? new JsonObject()
            : new JsonObject();

        if (body.ValueKind == JsonValueKind.Object)
        {
            foreach (var prop in body.EnumerateObject())
            {
                merged[prop.Name] = JsonNode.Parse(prop.Value.GetRawText());
            }
        }

        var mergedElement = JsonSerializer.SerializeToElement(merged);
        var envelope = await _api.UpdateAsync(module, id, mergedElement, ct);
        return Json(envelope);
    }

    [HttpDelete("{module}/{id:guid}")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Delete(string module, Guid id, CancellationToken ct)
    {
        if (!Modules.ContainsKey(module))
        {
            return NotFound();
        }

        var envelope = await _api.DeleteAsync(module, id, ct);
        return Json(envelope);
    }

    /// <summary>
    /// Bir iş kuralı satır eylemini (onayla/reddet/ters kayıt/mal kabul/kapat ...) ilgili
    /// API action uç noktasına iletir. Eylem beyaz listede (<see cref="RowActions"/>) yoksa
    /// 404 döner. Yetkilendirme API tarafında uç nokta-permission eşlemesiyle uygulanır.
    /// </summary>
    [HttpPost("{module}/action/{actionKey}/{id:guid}")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> RunAction(
        string module, string actionKey, Guid id,
        [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] JsonElement? body, CancellationToken ct)
    {
        if (!RowActions.TryGetValue((module.ToLowerInvariant(), actionKey.ToLowerInvariant()), out var descriptor))
        {
            return NotFound();
        }

        var path = descriptor.PathTemplate.Replace("{id}", id.ToString());

        if (string.Equals(descriptor.Method, "GET", StringComparison.OrdinalIgnoreCase))
        {
            var getResult = await _api.GetActionAsync(path, ct);
            return Json(getResult);
        }

        var hasBody = body is { ValueKind: JsonValueKind.Object or JsonValueKind.Array };
        var envelope = await _api.PostActionAsync(path, hasBody ? body : null, ct);
        return Json(envelope);
    }
}

