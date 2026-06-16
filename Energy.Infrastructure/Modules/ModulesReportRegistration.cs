using Microsoft.Extensions.DependencyInjection;

namespace Energy.Infrastructure.Modules;

/// <summary>Tüm rapor servislerinin (salt-okunur) DI kaydı.</summary>
public static class ModulesReportRegistration
{
    public static IServiceCollection AddModulesReportServices(this IServiceCollection services)
    {
        services.AddScoped<global::Energy.Application.Modules.Procurement.Reports.PurchaseOrderSummary.Services.IPurchaseOrderSummaryService, global::Energy.Infrastructure.Modules.Procurement.Reports.PurchaseOrderSummary.PurchaseOrderSummaryService>();
        services.AddScoped<global::Energy.Application.Modules.Inventory.Reports.StockBalanceReport.Services.IStockBalanceReportService, global::Energy.Infrastructure.Modules.Inventory.Reports.StockBalanceReport.StockBalanceReportService>();
        services.AddScoped<global::Energy.Application.Modules.Projects.Reports.ProjectStatusReport.Services.IProjectStatusReportService, global::Energy.Infrastructure.Modules.Projects.Reports.ProjectStatusReport.ProjectStatusReportService>();
        services.AddScoped<global::Energy.Application.Modules.HR.Reports.TimesheetSummary.Services.ITimesheetSummaryService, global::Energy.Infrastructure.Modules.HR.Reports.TimesheetSummary.TimesheetSummaryService>();
        services.AddScoped<global::Energy.Application.Modules.Finance.Reports.PayableAging.Services.IPayableAgingService, global::Energy.Infrastructure.Modules.Finance.Reports.PayableAging.PayableAgingService>();
        services.AddScoped<global::Energy.Application.Modules.Finance.Reports.ReceivableAging.Services.IReceivableAgingService, global::Energy.Infrastructure.Modules.Finance.Reports.ReceivableAging.ReceivableAgingService>();
        services.AddScoped<global::Energy.Application.Modules.ProgressPayments.Reports.ProgressPaymentSummary.Services.IProgressPaymentSummaryService, global::Energy.Infrastructure.Modules.ProgressPayments.Reports.ProgressPaymentSummary.ProgressPaymentSummaryService>();
        return services;
    }
}
