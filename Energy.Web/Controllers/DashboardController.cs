using Energy.Localization;
using Energy.Shared.Models.V1.Home.Requests;
using Energy.Web.Clients.Home;
using Energy.Web.Models.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Energy.Web.Controllers;

[Authorize]
[Route("[controller]")]
public sealed class DashboardController : Controller
{
    private readonly IHomeApiClient _homeApiClient;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public DashboardController(
        IHomeApiClient homeApiClient,
        IStringLocalizer<SharedResource> localizer)
    {
        _homeApiClient = homeApiClient;
        _localizer = localizer;
    }

    [HttpGet("/")]
    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewData["Title"] = _localizer.GetText(LocalizationKeys.Menus.Dashboard);

        var envelope = await _homeApiClient.GetDashboardAsync(
            new GetHomeDashboardRequest(),
            cancellationToken);

        if (!envelope.IsSuccess || envelope.Data is null)
        {
            return View(new DashboardIndexViewModel
            {
                ReadinessScore = 0,
                ConfiguredAreaCount = 0,
                StatusKey = LocalizationKeys.Dashboard.NeedsAttention,
                Metrics = Array.Empty<DashboardMetricViewModel>(),
                QuickLinks = Array.Empty<DashboardQuickLinkViewModel>()
            });
        }

        var data = envelope.Data;
        var statusKey = data.ReadinessScore >= 75
            ? LocalizationKeys.Dashboard.Ready
            : LocalizationKeys.Dashboard.NeedsAttention;

        var model = new DashboardIndexViewModel
        {
            ReadinessScore = data.ReadinessScore,
            ConfiguredAreaCount = data.ConfiguredAreaCount,
            StatusKey = statusKey,
            Metrics =
            [
                new DashboardMetricViewModel
                {
                    LabelKey = LocalizationKeys.Dashboard.ActiveUsers,
                    DescriptionKey = LocalizationKeys.Dashboard.ActiveUsersDescription,
                    Value = data.ActiveUsers.ToString()
                },
                new DashboardMetricViewModel
                {
                    LabelKey = LocalizationKeys.Dashboard.TotalRoles,
                    DescriptionKey = LocalizationKeys.Dashboard.TotalRolesDescription,
                    Value = data.TotalRoles.ToString()
                },
                new DashboardMetricViewModel
                {
                    LabelKey = LocalizationKeys.Dashboard.TotalPermissions,
                    DescriptionKey = LocalizationKeys.Dashboard.TotalPermissionsDescription,
                    Value = data.TotalPermissions.ToString()
                },
                new DashboardMetricViewModel
                {
                    LabelKey = LocalizationKeys.Dashboard.TotalMenus,
                    DescriptionKey = LocalizationKeys.Dashboard.TotalMenusDescription,
                    Value = data.TotalMenus.ToString()
                }
            ],
            QuickLinks = data.QuickLinks
                .Select(link => new DashboardQuickLinkViewModel
                {
                    Title = link.Name,
                    Url = link.Url,
                    Icon = string.IsNullOrEmpty(link.Icon) ? null : link.Icon
                })
                .ToArray()
        };

        return View(model);
    }
}

