using Energy.Application.Workflow.Services;
using Energy.Domain.Common;
using Energy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Energy.Infrastructure.Workflow.Services;

/// <summary>
/// Onay sonucunu bilinen kaynak belgelere yansıtan uygulama. Her belge türünün kendi
/// durum alanını (DocumentStatus / RequestStatus / PurchaseOrderStatus /
/// ApprovalRequestStatus) doğru biçimde günceller ve onay-talep bağlantısını yazar.
/// </summary>
public sealed class ApprovalSourceUpdater : IApprovalSourceUpdater
{
    private readonly AppDbContext _db;
    private readonly ILogger<ApprovalSourceUpdater> _logger;

    public ApprovalSourceUpdater(AppDbContext db, ILogger<ApprovalSourceUpdater> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task ApplyAsync(
        string relatedModule, string relatedEntityType, Guid entityId,
        Guid approvalRequestId, ApprovalOutcome outcome, CancellationToken ct = default)
    {
        switch (relatedEntityType)
        {
            case "Request":
            {
                var e = await _db.Requests.FirstOrDefaultAsync(x => x.Id == entityId, ct);
                if (e is not null) { e.Status = MapRequestStatus(outcome); e.ApprovalRequestId = approvalRequestId; }
                break;
            }
            case "PurchaseOrder":
            {
                var e = await _db.PurchaseOrders.FirstOrDefaultAsync(x => x.Id == entityId, ct);
                if (e is not null) { e.Status = MapPurchaseOrderStatus(outcome); e.ApprovalRequestId = approvalRequestId; }
                break;
            }
            case "StockDocument":
            {
                var e = await _db.StockDocuments.FirstOrDefaultAsync(x => x.Id == entityId, ct);
                if (e is not null) { e.Status = MapDocumentStatus(outcome); e.ApprovalRequestId = approvalRequestId; }
                break;
            }
            case "ExpenseClaim":
            {
                var e = await _db.ExpenseClaims.FirstOrDefaultAsync(x => x.Id == entityId, ct);
                if (e is not null) { e.Status = MapApprovalStatus(outcome); e.ApprovalRequestId = approvalRequestId; }
                break;
            }
            case "LeaveRequest":
            {
                var e = await _db.LeaveRequests.FirstOrDefaultAsync(x => x.Id == entityId, ct);
                if (e is not null) { e.Status = MapApprovalStatus(outcome); e.ApprovalRequestId = approvalRequestId; }
                break;
            }
            case "Timesheet":
            {
                var e = await _db.Timesheets.FirstOrDefaultAsync(x => x.Id == entityId, ct);
                if (e is not null) { e.Status = MapApprovalStatus(outcome); e.ApprovalRequestId = approvalRequestId; }
                break;
            }
            case "Payment":
            {
                var e = await _db.Payments.FirstOrDefaultAsync(x => x.Id == entityId, ct);
                if (e is not null) { e.Status = MapApprovalStatus(outcome); e.ApprovalRequestId = approvalRequestId; }
                break;
            }
            case "Collection":
            {
                var e = await _db.Collections.FirstOrDefaultAsync(x => x.Id == entityId, ct);
                if (e is not null) { e.Status = MapApprovalStatus(outcome); e.ApprovalRequestId = approvalRequestId; }
                break;
            }
            case "ProgressPayment":
            {
                var e = await _db.ProgressPayments.FirstOrDefaultAsync(x => x.Id == entityId, ct);
                if (e is not null) { e.Status = MapApprovalStatus(outcome); e.ApprovalRequestId = approvalRequestId; }
                break;
            }
            default:
                _logger.LogInformation(
                    "Approval source updater: no handler for {Module}/{Entity}; status not propagated.",
                    relatedModule, relatedEntityType);
                break;
        }
    }

    private static ApprovalRequestStatus MapApprovalStatus(ApprovalOutcome outcome) => outcome switch
    {
        ApprovalOutcome.Pending => ApprovalRequestStatus.Pending,
        ApprovalOutcome.Approved => ApprovalRequestStatus.Approved,
        ApprovalOutcome.Rejected => ApprovalRequestStatus.Rejected,
        ApprovalOutcome.Returned => ApprovalRequestStatus.Returned,
        ApprovalOutcome.Cancelled => ApprovalRequestStatus.Cancelled,
        _ => ApprovalRequestStatus.Pending,
    };

    private static RequestStatus MapRequestStatus(ApprovalOutcome outcome) => outcome switch
    {
        ApprovalOutcome.Pending => RequestStatus.PendingApproval,
        ApprovalOutcome.Approved => RequestStatus.Approved,
        ApprovalOutcome.Rejected => RequestStatus.Rejected,
        ApprovalOutcome.Returned => RequestStatus.Draft,
        ApprovalOutcome.Cancelled => RequestStatus.Draft,
        _ => RequestStatus.PendingApproval,
    };

    private static PurchaseOrderStatus MapPurchaseOrderStatus(ApprovalOutcome outcome) => outcome switch
    {
        ApprovalOutcome.Pending => PurchaseOrderStatus.Draft,
        ApprovalOutcome.Approved => PurchaseOrderStatus.Approved,
        ApprovalOutcome.Rejected => PurchaseOrderStatus.Draft,
        ApprovalOutcome.Returned => PurchaseOrderStatus.Draft,
        ApprovalOutcome.Cancelled => PurchaseOrderStatus.Cancelled,
        _ => PurchaseOrderStatus.Draft,
    };

    private static DocumentStatus MapDocumentStatus(ApprovalOutcome outcome) => outcome switch
    {
        ApprovalOutcome.Pending => DocumentStatus.PendingApproval,
        ApprovalOutcome.Approved => DocumentStatus.Approved,
        ApprovalOutcome.Rejected => DocumentStatus.Rejected,
        ApprovalOutcome.Returned => DocumentStatus.Draft,
        ApprovalOutcome.Cancelled => DocumentStatus.Cancelled,
        _ => DocumentStatus.PendingApproval,
    };
}

