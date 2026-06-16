using Energy.Web.Clients.Home;
using Energy.Web.Clients.Identity;
using Energy.Web.Clients.Chat;
using Energy.Web.Clients.Infrastructure.Authentication;
using Energy.Web.Clients.Infrastructure.ClientIdentity;
using Energy.Web.Clients.Localization;
using Energy.Web.Clients.Logger;
using Energy.Web.Clients;
using Energy.Web.Clients.Settings;
using Energy.Web.Configuration;
using Microsoft.Extensions.Options;
using SystemClients = Energy.Web.Clients.System;

namespace Energy.Web.Clients.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddEnergyApiClients(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IUserApiTokenProvider, UserApiTokenProvider>();
        // Singleton: sistem/servis hesabı jetonunu istekler arasında önbelleğe alır.
        services.AddSingleton<IServiceApiTokenProvider, ServiceApiTokenProvider>();
        services.AddTransient<AuthHeaderHandler>();
        services.AddScoped<BrowserClientIdService>();
        services.AddTransient<ClientIdentityHeaderHandler>();

        AddAnonymous<IAuthApiClient, AuthApiClient>(services);
        AddAuthenticated<IHomeApiClient, HomeApiClient>(services);
        AddAuthenticated<IUserApiClient, UserApiClient>(services);
        AddAuthenticated<IRoleApiClient, RoleApiClient>(services);
        AddAuthenticated<IPermissionApiClient, PermissionApiClient>(services);
        AddAuthenticated<SystemClients.IMenuApiClient, SystemClients.MenuApiClient>(services);
        AddAuthenticated<SystemClients.IApiEndpointApiClient, SystemClients.ApiEndpointApiClient>(services);
        AddAuthenticated<ILocalizationApiClient, LocalizationApiClient>(services);
        AddAuthenticated<IAuditLogIngestClient, AuditLogIngestClient>(services);
        AddAuthenticated<IAuditLogQueryClient, AuditLogQueryClient>(services);
        AddAuthenticated<IChatApiClient, ChatApiClient>(services);
        AddAuthenticated<ISettingsApiClient, SettingsApiClient>(services);

        // Tüm per-entity API istemcileri (IAM/Chat hariç).
        services.AddEntityApiClients();

        // ER Overview rapor API istemcileri.
        services.AddReportApiClients();

        // Süreç ekranı API istemcileri (standart süreç rotaları).
        AddAuthenticated<Energy.Web.Clients.Workflow.Processes.Approval.IApprovalProcessApiClient,
            Energy.Web.Clients.Workflow.Processes.Approval.ApprovalProcessApiClient>(services);
        AddAuthenticated<Energy.Web.Clients.Inventory.Processes.StockIssue.IStockIssueProcessApiClient,
            Energy.Web.Clients.Inventory.Processes.StockIssue.StockIssueProcessApiClient>(services);
        AddAuthenticated<Energy.Web.Clients.Inventory.Processes.StockTransfer.IStockTransferProcessApiClient,
            Energy.Web.Clients.Inventory.Processes.StockTransfer.StockTransferProcessApiClient>(services);
        AddAuthenticated<Energy.Web.Clients.Procurement.Processes.GoodsReceipt.IGoodsReceiptProcessApiClient,
            Energy.Web.Clients.Procurement.Processes.GoodsReceipt.GoodsReceiptProcessApiClient>(services);
        AddAuthenticated<Energy.Web.Clients.Finance.Processes.TimesheetCost.ITimesheetCostProcessApiClient,
            Energy.Web.Clients.Finance.Processes.TimesheetCost.TimesheetCostProcessApiClient>(services);
        AddAuthenticated<Energy.Web.Clients.Finance.Processes.ProgressPaymentPosting.IProgressPaymentPostingProcessApiClient,
            Energy.Web.Clients.Finance.Processes.ProgressPaymentPosting.ProgressPaymentPostingProcessApiClient>(services);

        // Belge dosya/versiyon yönetimi API istemcisi.
        AddAuthenticated<Energy.Web.Clients.Documents.Files.IDocumentFilesApiClient,
            Energy.Web.Clients.Documents.Files.DocumentFilesApiClient>(services);

        // Ödeme tahsis süreci API istemcisi.
        AddAuthenticated<Energy.Web.Clients.Finance.Processes.PaymentAllocation.IPaymentAllocationProcessApiClient,
            Energy.Web.Clients.Finance.Processes.PaymentAllocation.PaymentAllocationProcessApiClient>(services);
        return services;
    }

    private static void AddAuthenticated<TContract, TImpl>(IServiceCollection services)
        where TContract : class where TImpl : class, TContract
    {
        services.AddHttpClient<TContract, TImpl>(Configure)
            .ConfigurePrimaryHttpMessageHandler(CreatePrimaryHandler)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
    }

    private static void AddAnonymous<TContract, TImpl>(IServiceCollection services)
        where TContract : class where TImpl : class, TContract
    {
        services.AddHttpClient<TContract, TImpl>(Configure)
            .ConfigurePrimaryHttpMessageHandler(CreatePrimaryHandler)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>();
    }

    private static void Configure(IServiceProvider sp, HttpClient http)
    {
        var settings = sp.GetRequiredService<IOptions<ApiSettings>>().Value;
        if (string.IsNullOrWhiteSpace(settings.BaseUrl))
            throw new InvalidOperationException("Api:BaseUrl is not configured.");
        http.BaseAddress = new Uri(settings.BaseUrl);
    }

    private static HttpMessageHandler CreatePrimaryHandler(IServiceProvider sp)
    {
        var settings = sp.GetRequiredService<IOptions<ApiSettings>>().Value;
        var handler = new HttpClientHandler();

        // Geçersiz/kendinden imzalı API TLS sertifikaları için isteğe bağlı atlama. Yalnızca
        // yapılandırılan API sunucusu muaf tutulur; diğer her şey varsayılan kontrolleri korur.
        if (settings.AllowInvalidCertificate)
        {
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        }

        return handler;
    }
}
