using Energy.Localization;
using Energy.Shared.Identity.Permissions;
using Energy.Web.Clients.Home;
using Energy.Web.Common.Filters;
using Energy.Web.Models.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Energy.Web.Controllers;

[Authorize]
[PagePermission(PermissionCatalog.DashboardRead)]
[Route("[controller]")]
public sealed class DashboardController : Controller
{
    private readonly IHomeApiClient _home;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public DashboardController(IHomeApiClient home, IStringLocalizer<SharedResource> localizer)
    {
        _home = home;
        _localizer = localizer;
    }

    [HttpGet("/")]
    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        ViewData["Title"] = _localizer.GetText(LocalizationKeys.Menus.Dashboard);

        DashboardIndexViewModel BuildEmpty() => new()
        {
            ReadinessScore = 0,
            ConfiguredAreaCount = 0,
            StatusKey = LocalizationKeys.Dashboard.NeedsAttention,
            Metrics = Array.Empty<DashboardMetricViewModel>(),
            EnterpriseMetrics = Array.Empty<EnterpriseMetricViewModel>(),
            QuickLinks = Array.Empty<DashboardQuickLinkViewModel>()
        };

        var envelope = await _home.GetDashboardAsync(ct);
        if (!envelope.IsSuccess || envelope.Data is null)
        {
            return View(BuildEmpty());
        }

        var d = envelope.Data;

        // Kurumsal (iş) modül widget'larının canlı metriklerini al; yetki API tarafında
        // (DashboardRead + her widget'ın gerektirdiği yetki) süzülür. Hata olursa boş geç.
        var enterpriseMetrics = Array.Empty<EnterpriseMetricViewModel>().AsEnumerable();
        var metricsEnvelope = await _home.GetEnterpriseMetricsAsync(ct);
        if (metricsEnvelope.IsSuccess && metricsEnvelope.Data is { Count: > 0 } items)
        {
            enterpriseMetrics = items
                .OrderBy(m => m.DisplayOrder)
                .Select(m => new EnterpriseMetricViewModel
                {
                    NameKey = m.NameKey,
                    DescriptionKey = m.DescriptionKey,
                    Module = m.Module,
                    Value = m.Value.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture),
                    Url = ResolveModuleUrl(m.Module)
                })
                .ToList();
        }

        // API'nin döndürdüğü altı sayaç üzerinden basit bir "hazırlık" görünümü oluştur:
        // en az bir yapılandırılmış varlığı olan her alan hazır sayılır.
        var areas = new[]
        {
            d.ActiveUsers > 0,
            d.TotalRoles > 0,
            d.TotalPermissions > 0,
            d.TotalMenus > 0,
            d.TotalApiEndpoints > 0
        };
        var configured = areas.Count(x => x);
        var score = (int)Math.Round(configured * 100.0 / areas.Length);
        var statusKey = score >= 75
            ? LocalizationKeys.Dashboard.Ready
            : LocalizationKeys.Dashboard.NeedsAttention;

        var model = new DashboardIndexViewModel
        {
            ReadinessScore = score,
            ConfiguredAreaCount = configured,
            StatusKey = statusKey,
            Metrics = new[]            {
                new DashboardMetricViewModel
                {
                    LabelKey = LocalizationKeys.Dashboard.ActiveUsers,
                    DescriptionKey = LocalizationKeys.Dashboard.ActiveUsersDescription,
                    Value = d.ActiveUsers.ToString()
                },
                new DashboardMetricViewModel
                {
                    LabelKey = LocalizationKeys.Dashboard.TotalRoles,
                    DescriptionKey = LocalizationKeys.Dashboard.TotalRolesDescription,
                    Value = d.TotalRoles.ToString()
                },
                new DashboardMetricViewModel
                {
                    LabelKey = LocalizationKeys.Dashboard.TotalPermissions,
                    DescriptionKey = LocalizationKeys.Dashboard.TotalPermissionsDescription,
                    Value = d.TotalPermissions.ToString()
                },
                new DashboardMetricViewModel
                {
                    LabelKey = LocalizationKeys.Dashboard.TotalMenus,
                    DescriptionKey = LocalizationKeys.Dashboard.TotalMenusDescription,
                    Value = d.TotalMenus.ToString()
                }
            },
            EnterpriseMetrics = enterpriseMetrics.ToArray(),
            QuickLinks = new[]
            {
                new DashboardQuickLinkViewModel { Title = LocalizationKeys.Menus.Users,         Url = "/users",          Icon = "user" },
                new DashboardQuickLinkViewModel { Title = LocalizationKeys.Menus.Roles,         Url = "/roles",          Icon = "group" },
                new DashboardQuickLinkViewModel { Title = LocalizationKeys.Menus.Menus_,        Url = "/menus",          Icon = "menu" },
                new DashboardQuickLinkViewModel { Title = LocalizationKeys.Menus.Permissions,   Url = "/permissions",    Icon = "key" },
                new DashboardQuickLinkViewModel { Title = LocalizationKeys.Menus.Localization,  Url = "/localization",   Icon = "globe" },
                new DashboardQuickLinkViewModel { Title = LocalizationKeys.Menus.Profile,       Url = "/profile",        Icon = "user" }
            }
        };

        return View(model);
    }

    /// <summary>Widget'ın iş modülünü, generic CRUD ekranının rota segmentine eşler.</summary>
    private static string? ResolveModuleUrl(string module) => module switch
    {
        "Inventory" => "/m/inventory",
        "Workflow" => "/m/workflow",
        "Budget" => "/m/budget",
        "Procurement" => "/m/procurement",
        "Operations" => "/m/operations",
        _ => null,
    };
}
