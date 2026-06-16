using Energy.Shared.Common;
using Energy.Domain.IAM;
using Energy.Domain.Core;
using Energy.Domain.Projects;
using Energy.Domain.BusinessPartners;
using Energy.Domain.Catalog;
using Energy.Domain.Operations;
using Energy.Domain.Procurement;
using Energy.Domain.Workflow;
using Energy.Domain.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Energy.Infrastructure.Seeding;

/// <summary>
/// "1 aylık tam kapasite" demo verisi. <see cref="EnsureFullSampleDataAsync"/> her
/// tabloya en az bir kayıt eklerken; bu katman son 30 güne yayılmış, <b>tüm durum
/// (case) varyasyonlarını</b> içeren hacimli operasyonel kayıtlar üretir; böylece
/// grid/rapor/onay ekranları gerçekçi ve dolu görünür.
///
/// <para>Üretilen kümeler (hepsi idempotent — doğal kod/işaretçi ile korunur):</para>
/// <list type="bullet">
///   <item>Satın alma siparişleri: 24 adet, 5 durumun tamamı (Taslak → İptal).</item>
///   <item>İş emirleri: 12 adet, 6 durumun tamamı (Taslak → Kapalı).</item>
///   <item>Onay talepleri: 6 durumun tamamı + uygun onay/ret/iade/iptal hareketi.</item>
///   <item>Bildirimler: aya yayılmış okunmuş/okunmamış kayıtlar.</item>
/// </list>
/// </summary>
public sealed partial class SystemSeeder
{
    private static readonly PurchaseOrderStatus[] DemoPoStatuses =
    {
        PurchaseOrderStatus.Draft, PurchaseOrderStatus.Approved, PurchaseOrderStatus.PartiallyReceived,
        PurchaseOrderStatus.Received, PurchaseOrderStatus.Cancelled,
    };

    private static readonly WorkOrderStatus[] DemoWoStatuses =
    {
        WorkOrderStatus.Draft, WorkOrderStatus.Assigned, WorkOrderStatus.InProgress,
        WorkOrderStatus.OnHold, WorkOrderStatus.Completed, WorkOrderStatus.Closed,
    };

    private static readonly ApprovalRequestStatus[] DemoApprovalStates =
    {
        ApprovalRequestStatus.Draft, ApprovalRequestStatus.Pending, ApprovalRequestStatus.Approved,
        ApprovalRequestStatus.Rejected, ApprovalRequestStatus.Returned, ApprovalRequestStatus.Cancelled,
    };

    /// <summary>Son 30 güne yayılmış, tüm case'leri kapsayan hacimli demo veriyi üretir.</summary>
    private async Task EnsureDemoMonthDataAsync(CancellationToken ct)
    {
        var admin = await _db.Users.FirstOrDefaultAsync(u => u.UserName == "admin", ct);
        var currency = await _db.Currencies.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Code == "TRY", ct);
        var project = await _db.Projects.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Code == "PRJ-001", ct);
        var supplier = await _db.BusinessPartners.IgnoreQueryFilters().FirstOrDefaultAsync(b => b.Code == "SUP-001", ct);
        var material = await _db.Materials.IgnoreQueryFilters().FirstOrDefaultAsync(m => m.Code == "MAT-001", ct);
        var workOrderType = await _db.WorkOrderTypes.IgnoreQueryFilters().FirstOrDefaultAsync(t => t.Code == "WOT-001", ct);
        var approvalVersion = await _db.ApprovalDefinitionVersions.IgnoreQueryFilters()
            .OrderBy(v => v.VersionNo).FirstOrDefaultAsync(ct);

        if (admin is null || currency is null || project is null || supplier is null ||
            material is null || workOrderType is null)
        {
            _logger.LogWarning("Demo month data skipped: one or more anchor records are missing.");
            return;
        }

        var monthStart = DateTime.UtcNow.Date.AddDays(-29);

        await SeedDemoPurchaseOrdersAsync(monthStart, supplier, project, currency, material, ct);
        await SeedDemoWorkOrdersAsync(monthStart, workOrderType, project, ct);
        if (approvalVersion is not null)
        {
            await SeedDemoApprovalsAsync(monthStart, approvalVersion, admin, ct);
        }
        await SeedDemoNotificationsAsync(monthStart, admin, ct);

        _logger.LogInformation("Demo month data: high-volume, all-status operational records seeded.");
    }

    // 24 satın alma siparişi — 5 durumun tamamı, aya yayılı, her birinde satır.
    private async Task SeedDemoPurchaseOrdersAsync(
        DateTime monthStart, BusinessPartner supplier, Project project, Currency currency, Material material, CancellationToken ct)
    {
        for (var i = 1; i <= 24; i++)
        {
            var code = $"PO-D{i:00}";
            var status = DemoPoStatuses[i % DemoPoStatuses.Length];
            var orderDate = monthStart.AddDays(i % 30);

            var po = await GetOrAddAsync(_db.PurchaseOrders, o => o.OrderNo == code, () => new PurchaseOrder
            {
                Id = Guid.NewGuid(), SupplierId = supplier.Id, ProjectId = project.Id, CurrencyId = currency.Id,
                Status = status, OrderNo = code, OrderDate = orderDate,
            }, ct);

            var qty = 10m + i;
            var received = status switch
            {
                PurchaseOrderStatus.Received => qty,
                PurchaseOrderStatus.PartiallyReceived => Math.Round(qty / 2m, 2),
                _ => 0m,
            };

            await EnsureAsync(_db.PurchaseOrderLines, l => l.PurchaseOrderId == po.Id, () => new PurchaseOrderLine
            {
                Id = Guid.NewGuid(), PurchaseOrderId = po.Id, MaterialId = material.Id,
                Quantity = qty, UnitPrice = 100m + i, CurrencyId = currency.Id, ReceivedQuantity = received,
            }, ct);
        }
        await _db.SaveChangesAsync(ct);
    }

    // 12 iş emri — 6 durumun tamamı, aya yayılı planlı tarihlerle.
    private async Task SeedDemoWorkOrdersAsync(
        DateTime monthStart, WorkOrderType workOrderType, Project project, CancellationToken ct)
    {
        for (var i = 1; i <= 12; i++)
        {
            var code = $"WO-D{i:00}";
            var status = DemoWoStatuses[i % DemoWoStatuses.Length];
            var start = monthStart.AddDays((i * 2) % 30);

            await GetOrAddAsync(_db.WorkOrders, w => w.WorkOrderNo == code, () => new WorkOrder
            {
                Id = Guid.NewGuid(), WorkOrderTypeId = workOrderType.Id, ProjectId = project.Id,
                Status = status, WorkOrderNo = code, Title = $"Demo İş Emri {i:00}",
                Description = $"Aylık demo iş emri ({status}).",
                PlannedStart = start, PlannedEnd = start.AddDays(2),
            }, ct);
        }
        await _db.SaveChangesAsync(ct);
    }

    // Onay talepleri — 6 durumun tamamı + terminal durumlar için ilgili onay hareketi.
    private async Task SeedDemoApprovalsAsync(
        DateTime monthStart, ApprovalDefinitionVersion version, User admin, CancellationToken ct)
    {
        for (var i = 0; i < DemoApprovalStates.Length; i++)
        {
            var state = DemoApprovalStates[i];
            var marker = $"DemoApproval-{state}";

            var request = await GetOrAddAsync(_db.ApprovalRequests,
                r => r.RelatedModule == "DemoMonth" && r.RelatedEntityType == marker,
                () => new ApprovalRequest
                {
                    Id = Guid.NewGuid(), ApprovalDefinitionVersionId = version.Id,
                    RelatedModule = "DemoMonth", RelatedEntityType = marker, RelatedEntityId = Guid.NewGuid(),
                    RequestedByUserId = admin.Id, Status = state, CurrentStepNo = 1,
                }, ct);

            var actionType = state switch
            {
                ApprovalRequestStatus.Approved => (ApprovalActionType?)ApprovalActionType.Approve,
                ApprovalRequestStatus.Rejected => ApprovalActionType.Reject,
                ApprovalRequestStatus.Returned => ApprovalActionType.Return,
                ApprovalRequestStatus.Cancelled => ApprovalActionType.Cancel,
                _ => null,
            };

            if (actionType is not null)
            {
                await EnsureAsync(_db.ApprovalActions, a => a.ApprovalRequestId == request.Id, () => new ApprovalAction
                {
                    Id = Guid.NewGuid(), ApprovalRequestId = request.Id, UserId = admin.Id,
                    ActionType = actionType.Value, ActionAt = monthStart.AddDays(i * 4),
                    Note = $"Demo {actionType} hareketi.",
                }, ct);
            }
        }
        await _db.SaveChangesAsync(ct);
    }

    // 15 bildirim — aya yayılı, okunmuş/okunmamış karışık.
    private async Task SeedDemoNotificationsAsync(DateTime monthStart, User admin, CancellationToken ct)
    {
        for (var i = 1; i <= 15; i++)
        {
            var title = $"DEMO-N{i:00}";
            var read = i % 3 == 0;

            var notification = await GetOrAddAsync(_db.Notifications, n => n.Title == title, () => new Notification
            {
                Id = Guid.NewGuid(), Title = title, Body = $"Aylık demo bildirim {i:00}.",
                NotificationType = i % 2 == 0 ? "Info" : "Warning", RelatedModule = "DemoMonth",
            }, ct);

            await EnsureAsync(_db.NotificationRecipients,
                r => r.NotificationId == notification.Id && r.UserId == admin.Id,
                () => new NotificationRecipient
                {
                    Id = Guid.NewGuid(), NotificationId = notification.Id, UserId = admin.Id,
                    IsRead = read, ReadAt = read ? monthStart.AddDays(i) : null,
                }, ct);
        }
        await _db.SaveChangesAsync(ct);
    }
}

