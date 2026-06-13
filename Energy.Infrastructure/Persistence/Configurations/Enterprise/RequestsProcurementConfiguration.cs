using Energy.Domain.BusinessPartners;
using Energy.Domain.Catalog;
using Energy.Domain.Core;
using Energy.Domain.Identity;
using Energy.Domain.Inventory;
using Energy.Domain.Procurement;
using Energy.Domain.Projects;
using Energy.Domain.Requests;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Enterprise;

/// <summary>Requests ve Procurement modülleri EF Core yapılandırması.</summary>
public static class RequestsProcurementConfiguration
{
    public static void Configure(ModelBuilder b)
    {
        // ---- Requests ----
        b.Entity<RequestType>(e => { e.ToTable("RequestTypes"); e.HasIndex(x => x.Code).IsUnique(); });

        b.Entity<Request>(e =>
        {
            e.ToTable("Requests");
            e.HasIndex(x => x.RequestNo).IsUnique();
            e.HasOne<RequestType>().WithMany().HasForeignKey(x => x.RequestTypeId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<User>().WithMany().HasForeignKey(x => x.RequestedByUserId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<RequestLine>(e =>
        {
            e.ToTable("RequestLines");
            e.HasOne<Request>().WithMany().HasForeignKey(x => x.RequestId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Material>().WithMany().HasForeignKey(x => x.MaterialId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<UnitOfMeasure>().WithMany().HasForeignKey(x => x.UnitOfMeasureId).OnDelete(DeleteBehavior.Restrict);
        });

        // ---- Procurement ----
        b.Entity<SupplierQuote>(e =>
        {
            e.ToTable("SupplierQuotes");
            e.HasIndex(x => x.QuoteNo).IsUnique();
            e.HasOne<BusinessPartner>().WithMany().HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<SupplierQuoteLine>(e =>
        {
            e.ToTable("SupplierQuoteLines");
            e.HasOne<SupplierQuote>().WithMany().HasForeignKey(x => x.SupplierQuoteId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<RequestLine>().WithMany().HasForeignKey(x => x.RequestLineId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Material>().WithMany().HasForeignKey(x => x.MaterialId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<PurchaseOrder>(e =>
        {
            e.ToTable("PurchaseOrders");
            e.HasIndex(x => x.OrderNo).IsUnique();
            e.HasOne<BusinessPartner>().WithMany().HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<PurchaseOrderLine>(e =>
        {
            e.ToTable("PurchaseOrderLines");
            e.HasOne<PurchaseOrder>().WithMany().HasForeignKey(x => x.PurchaseOrderId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<RequestLine>().WithMany().HasForeignKey(x => x.RequestLineId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Material>().WithMany().HasForeignKey(x => x.MaterialId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<PurchaseReceipt>(e =>
        {
            e.ToTable("PurchaseReceipts");
            e.HasIndex(x => x.ReceiptNo).IsUnique();
            e.HasOne<BusinessPartner>().WithMany().HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<PurchaseOrder>().WithMany().HasForeignKey(x => x.PurchaseOrderId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.WarehouseId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<StockDocument>().WithMany().HasForeignKey(x => x.StockDocumentId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<PurchaseReceiptLine>(e =>
        {
            e.ToTable("PurchaseReceiptLines");
            e.HasOne<PurchaseReceipt>().WithMany().HasForeignKey(x => x.PurchaseReceiptId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<PurchaseOrderLine>().WithMany().HasForeignKey(x => x.PurchaseOrderLineId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Material>().WithMany().HasForeignKey(x => x.MaterialId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<SupplierInvoice>(e =>
        {
            e.ToTable("SupplierInvoices");
            e.HasIndex(x => x.InvoiceNo).IsUnique();
            e.HasOne<BusinessPartner>().WithMany().HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<PurchaseOrder>().WithMany().HasForeignKey(x => x.PurchaseOrderId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<PurchaseReceipt>().WithMany().HasForeignKey(x => x.PurchaseReceiptId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<SupplierInvoiceLine>(e =>
        {
            e.ToTable("SupplierInvoiceLines");
            e.HasOne<SupplierInvoice>().WithMany().HasForeignKey(x => x.SupplierInvoiceId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<Material>().WithMany().HasForeignKey(x => x.MaterialId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}

