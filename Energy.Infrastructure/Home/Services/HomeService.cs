using Energy.Application.Home.Services;
using Energy.Infrastructure.Persistence;
using Energy.Localization;
using Energy.Shared.Models.V1.Home.Requests;
using Energy.Shared.Models.V1.Home.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Energy.Infrastructure.Home.Services;

public sealed class HomeService : IHomeService
{
    private readonly AppDbContext _dbContext;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public HomeService(AppDbContext dbContext, IStringLocalizer<SharedResource> localizer)
    {
        _dbContext = dbContext;
        _localizer = localizer;
    }

    public async Task<HomeDashboardResponse> GetDashboardAsync(GetHomeDashboardRequest request, CancellationToken cancellationToken = default)
    {
        var quickLinkCount = request.IncludeQuickLinks ? request.QuickLinkCount : 0;

        var activeUsers = await _dbContext.Users.AsNoTracking().CountAsync(user => user.IsActive, cancellationToken);
        var totalRoles = await _dbContext.Roles.AsNoTracking().CountAsync(cancellationToken);
        var totalPermissions = await _dbContext.Permissions.AsNoTracking().CountAsync(cancellationToken);
        var totalMenus = await _dbContext.Menus.AsNoTracking().CountAsync(cancellationToken);

        IReadOnlyList<HomeQuickLinkResponse> quickLinks;
        if (quickLinkCount > 0)
        {
            var quickLinkMenus = await _dbContext.Menus
                .AsNoTracking()
                .Where(menu => !_dbContext.Menus.Any(child => child.ParentId == menu.Id))
                .OrderBy(menu => menu.Order)
                .ThenBy(menu => menu.Name)
                .Take(quickLinkCount)
                .Select(menu => new
                {
                    menu.Name,
                    menu.Url,
                    menu.Icon
                })
                .ToListAsync(cancellationToken);

            quickLinks = quickLinkMenus
                .Select(menu => new HomeQuickLinkResponse
                {
                    Name = ResolveDisplayName(menu.Name),
                    Url = menu.Url,
                    Icon = menu.Icon
                })
                .ToList();
        }
        else
        {
            quickLinks = Array.Empty<HomeQuickLinkResponse>();
        }

        var configuredAreaCount = new[]
        {
            activeUsers > 0,
            totalRoles > 0,
            totalPermissions > 0,
            totalMenus > 0
        }.Count(isConfigured => isConfigured);

        return new HomeDashboardResponse
        {
            ActiveUsers = activeUsers,
            TotalRoles = totalRoles,
            TotalPermissions = totalPermissions,
            TotalMenus = totalMenus,
            ReadinessScore = configuredAreaCount * 25,
            ConfiguredAreaCount = configuredAreaCount,
            QuickLinks = quickLinks
        };
    }

    private string ResolveDisplayName(string nameOrKey)
    {
        if (string.IsNullOrWhiteSpace(nameOrKey))
        {
            return string.Empty;
        }

        var localized = _localizer[nameOrKey];
        return localized.ResourceNotFound ? nameOrKey : localized.Value;
    }
}

