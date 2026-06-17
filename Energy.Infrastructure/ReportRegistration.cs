using Microsoft.Extensions.DependencyInjection;

namespace Energy.Infrastructure;

/// <summary>Tüm rapor servislerinin (salt-okunur) DI kaydı.</summary>
public static class ReportRegistration
{
    public static IServiceCollection AddReportServices(this IServiceCollection services)
    {
        services.AddScoped<global::Energy.Application.Procurement.Reports.PurchaseOrderSummary.Services.IPurchaseOrderSummaryService, global::Energy.Infrastructure.Procurement.Reports.PurchaseOrderSummary.PurchaseOrderSummaryService>();
        services.AddScoped<global::Energy.Application.Inventory.Reports.StockBalanceReport.Services.IStockBalanceReportService, global::Energy.Infrastructure.Inventory.Reports.StockBalanceReport.StockBalanceReportService>();
        services.AddScoped<global::Energy.Application.Projects.Reports.ProjectStatusReport.Services.IProjectStatusReportService, global::Energy.Infrastructure.Projects.Reports.ProjectStatusReport.ProjectStatusReportService>();
        services.AddScoped<global::Energy.Application.HR.Reports.TimesheetSummary.Services.ITimesheetSummaryService, global::Energy.Infrastructure.HR.Reports.TimesheetSummary.TimesheetSummaryService>();
        services.AddScoped<global::Energy.Application.Finance.Reports.PayableAging.Services.IPayableAgingService, global::Energy.Infrastructure.Finance.Reports.PayableAging.PayableAgingService>();
        services.AddScoped<global::Energy.Application.Finance.Reports.ReceivableAging.Services.IReceivableAgingService, global::Energy.Infrastructure.Finance.Reports.ReceivableAging.ReceivableAgingService>();
        services.AddScoped<global::Energy.Application.ProgressPayments.Reports.ProgressPaymentSummary.Services.IProgressPaymentSummaryService, global::Energy.Infrastructure.ProgressPayments.Reports.ProgressPaymentSummary.ProgressPaymentSummaryService>();
        return services;
    }
}
