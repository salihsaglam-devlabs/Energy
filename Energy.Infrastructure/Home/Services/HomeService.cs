using Energy.Shared.Common;
using Energy.Application.Home.Services;
using Energy.Application.Identity.Services;
using Energy.Domain.Common;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Home.Requests;
using Energy.Shared.Models.V1.Home.Responses;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Home.Services;

public sealed class HomeService : IHomeService
{
    private readonly AppDbContext _db;
    private readonly ICurrentUser _currentUser;
    private readonly IPermissionResolver _permissions;

    public HomeService(AppDbContext db, ICurrentUser currentUser, IPermissionResolver permissions)
    {
        _db = db;
        _currentUser = currentUser;
        _permissions = permissions;
    }

    public async Task<HomeDashboardResponse> GetDashboardAsync(GetHomeDashboardRequest request, CancellationToken ct = default)
    {
        var since = DateTime.UtcNow.AddHours(-24);
        return new HomeDashboardResponse
        {
            ActiveUsers = await _db.Users.AsNoTracking().CountAsync(u => u.IsActive, ct),
            TotalRoles = await _db.Roles.AsNoTracking().CountAsync(ct),
            TotalPermissions = await _db.Permissions.AsNoTracking().CountAsync(ct),
            TotalMenus = await _db.Menus.AsNoTracking().CountAsync(ct),
            TotalApiEndpoints = await _db.ApiEndpoints.AsNoTracking().CountAsync(ct),
            FailedLogins24h = await _db.AuditLogs.AsNoTracking().CountAsync(l => l.OccurredAt >= since && l.StatusCode == 401, ct)
        };
    }

    public async Task<IReadOnlyList<EnterpriseMetricResponse>> GetEnterpriseMetricsAsync(CancellationToken ct = default)
    {
        var widgets = await _db.DashboardWidgets.AsNoTracking()
            .Where(w => w.IsActive)
            .OrderBy(w => w.DisplayOrder)
            .ToListAsync(ct);

        if (widgets.Count == 0)
        {
            return Array.Empty<EnterpriseMetricResponse>();
        }

        // Çağıranın etkin yetki kümesini bir kez çözüp her widget için kontrol et.
        var granted = _currentUser.UserId is { } userId
            ? await _permissions.GetPermissionsAsync(userId, ct)
            : (IReadOnlySet<string>)new HashSet<string>();

        var result = new List<EnterpriseMetricResponse>(widgets.Count);
        foreach (var widget in widgets)
        {
            // Widget bir yetki gerektiriyorsa ve kullanıcıda yoksa atla (bilgi sızıntısını önle).
            if (!string.IsNullOrWhiteSpace(widget.RequiredPermissionCode) &&
                !granted.Contains(widget.RequiredPermissionCode))
            {
                continue;
            }

            var value = await ComputeValueAsync(widget.Code, ct);
            if (value is null)
            {
                continue; // Bilinmeyen widget kodu — sessizce atla.
            }

            result.Add(new EnterpriseMetricResponse
            {
                Code = widget.Code,
                Module = widget.Module,
                NameKey = widget.Name,
                DescriptionKey = $"DashboardWidgets.{widget.Code}.Description",
                WidgetType = widget.WidgetType,
                Value = value.Value,
                DisplayOrder = widget.DisplayOrder,
            });
        }

        return result;
    }

    /// <summary>Widget koduna göre canlı metrik değerini hesaplar; bilinmeyen kod için null döner.</summary>
    private async Task<decimal?> ComputeValueAsync(string code, CancellationToken ct) => code switch
    {
        // Kullanılabilir miktarı (Quantity - ReservedQuantity) sıfır ve altında olan stok bakiyesi sayısı.
        "LowStock" => await _db.StockBalances.AsNoTracking()
            .CountAsync(b => b.Quantity - b.ReservedQuantity <= 0m, ct),

        // Bekleyen (Pending) onay talebi sayısı.
        "PendingApprovals" => await _db.ApprovalRequests.AsNoTracking()
            .CountAsync(a => a.Status == ApprovalRequestStatus.Pending, ct),

        // Bütçe aşımındaki (gerçekleşen > planlanan) etkin bütçe sayısı.
        "BudgetOverrun" => await CountBudgetOverrunsAsync(ct),

        // Teslim bekleyen (onaylı veya kısmen teslim alınmış) satın alma siparişi sayısı.
        "OrderDelivery" => await _db.PurchaseOrders.AsNoTracking()
            .CountAsync(o => o.Status == PurchaseOrderStatus.Approved ||
                             o.Status == PurchaseOrderStatus.PartiallyReceived, ct),

        // Açık (tamamlanmamış/kapanmamış) iş emri sayısı.
        "WorkOrderProgress" => await _db.WorkOrders.AsNoTracking()
            .CountAsync(w => w.Status != WorkOrderStatus.Completed &&
                             w.Status != WorkOrderStatus.Closed, ct),

        _ => null,
    };

    /// <summary>
    /// Bütçe aşımındaki etkin bütçeleri sayar. FinanceService ile aynı kuralı uygular:
    /// planlanan = bütçe satırları toplamı; gerçekleşen = bütçenin proje/masraf merkezine
    /// bağlı finansal hareket satırları toplamı; gerçekleşen > planlanan ise aşım sayılır.
    /// </summary>
    private async Task<decimal> CountBudgetOverrunsAsync(CancellationToken ct)
    {
        var budgets = await _db.Budgets.AsNoTracking()
            .Where(b => b.IsActive)
            .Select(b => new { b.Id, b.ProjectId, b.CostCenterId })
            .ToListAsync(ct);

        var overrun = 0;
        foreach (var budget in budgets)
        {
            if (budget.ProjectId is null && budget.CostCenterId is null)
            {
                continue;
            }

            var planned = await _db.BudgetLines.AsNoTracking()
                .Where(l => l.BudgetId == budget.Id)
                .SumAsync(l => (decimal?)l.PlannedAmount, ct) ?? 0m;

            var actualQuery = _db.FinancialTransactionLines.AsNoTracking();
            actualQuery = budget.ProjectId is { } projectId
                ? actualQuery.Where(l => l.ProjectId == projectId)
                : actualQuery.Where(l => l.CostCenterId == budget.CostCenterId);

            var actual = await actualQuery.SumAsync(l => (decimal?)l.Amount, ct) ?? 0m;
            if (actual > planned)
            {
                overrun++;
            }
        }

        return overrun;
    }
}
