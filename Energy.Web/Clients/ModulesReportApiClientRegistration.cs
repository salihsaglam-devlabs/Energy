using Energy.Web.Clients.Infrastructure.Authentication;
using Energy.Web.Clients.Infrastructure.ClientIdentity;
using Energy.Web.Configuration;
using Microsoft.Extensions.Options;

namespace Energy.Web.Clients;

/// <summary>Tüm rapor API istemcilerinin (typed HttpClient) kaydı.</summary>
public static class ModulesReportApiClientRegistration
{
    public static IServiceCollection AddModulesReportApiClients(this IServiceCollection services)
    {
        services.AddHttpClient<global::Energy.Web.Clients.Procurement.Reports.PurchaseOrderSummary.IPurchaseOrderSummaryApiClient, global::Energy.Web.Clients.Procurement.Reports.PurchaseOrderSummary.PurchaseOrderSummaryApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Inventory.Reports.StockBalanceReport.IStockBalanceReportApiClient, global::Energy.Web.Clients.Inventory.Reports.StockBalanceReport.StockBalanceReportApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Projects.Reports.ProjectStatusReport.IProjectStatusReportApiClient, global::Energy.Web.Clients.Projects.Reports.ProjectStatusReport.ProjectStatusReportApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.HR.Reports.TimesheetSummary.ITimesheetSummaryApiClient, global::Energy.Web.Clients.HR.Reports.TimesheetSummary.TimesheetSummaryApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Finance.Reports.PayableAging.IPayableAgingApiClient, global::Energy.Web.Clients.Finance.Reports.PayableAging.PayableAgingApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Finance.Reports.ReceivableAging.IReceivableAgingApiClient, global::Energy.Web.Clients.Finance.Reports.ReceivableAging.ReceivableAgingApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.ProgressPayments.Reports.ProgressPaymentSummary.IProgressPaymentSummaryApiClient, global::Energy.Web.Clients.ProgressPayments.Reports.ProgressPaymentSummary.ProgressPaymentSummaryApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        return services;
    }

    private static void Configure(IServiceProvider sp, HttpClient http)
    {
        var settings = sp.GetRequiredService<IOptions<ApiSettings>>().Value;
        if (string.IsNullOrWhiteSpace(settings.BaseUrl))
            throw new InvalidOperationException("Api:BaseUrl is not configured.");
        http.BaseAddress = new Uri(settings.BaseUrl);
    }
}
