namespace Energy.Infrastructure.System.Services;

/// <summary>
/// Üretilen rapor API uç noktalarının (Controller.Action) rapor yetkilerine
/// eşlemesi. ApiEndpointSyncService başlangıçta bunları etkinleştirir.
/// </summary>
public static class ModulesReportEndpointPermissionMap
{
    public static void Apply(IDictionary<string, string?> map)
    {
        map["PurchaseOrderSummary.GetData"] = "Procurement.PurchaseOrderSummary.Read";
        map["PurchaseOrderSummary.Export"] = "Procurement.PurchaseOrderSummary.Export";
        map["StockBalanceReport.GetData"] = "Inventory.StockBalanceReport.Read";
        map["StockBalanceReport.Export"] = "Inventory.StockBalanceReport.Export";
        map["ProjectStatusReport.GetData"] = "Projects.ProjectStatusReport.Read";
        map["ProjectStatusReport.Export"] = "Projects.ProjectStatusReport.Export";
        map["TimesheetSummary.GetData"] = "HR.TimesheetSummary.Read";
        map["TimesheetSummary.Export"] = "HR.TimesheetSummary.Export";
        map["PayableAging.GetData"] = "Finance.PayableAging.Read";
        map["PayableAging.Export"] = "Finance.PayableAging.Export";
        map["ReceivableAging.GetData"] = "Finance.ReceivableAging.Read";
        map["ReceivableAging.Export"] = "Finance.ReceivableAging.Export";
        map["ProgressPaymentSummary.GetData"] = "ProgressPayments.ProgressPaymentSummary.Read";
        map["ProgressPaymentSummary.Export"] = "ProgressPayments.ProgressPaymentSummary.Export";
    }
}
