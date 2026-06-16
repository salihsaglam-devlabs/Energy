using Energy.Domain.BusinessPartners;
using Energy.Domain.Budget;
using Energy.Domain.Contracts;
using Energy.Domain.Core;
using Energy.Domain.FieldOperations;
using Energy.Domain.Finance;
using Energy.Domain.ProgressPayments;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Enterprise;

/// <summary>Finance, Budget, Contracts ve ProgressPayments modülleri EF Core yapılandırması.</summary>
public static class FinanceBudgetContractsProgressConfiguration
{
    public static void Configure(ModelBuilder b)
    {
        // ---- Finance ----
        b.Entity<FinancialAccount>(e =>
        {
            e.ToTable("FinancialAccounts");
            e.HasIndex(x => x.Code).IsUnique();
            e.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<CostCenter>(e =>
        {
            e.ToTable("CostCenters");
            e.HasIndex(x => x.Code).IsUnique();
            e.HasOne<CostCenter>().WithMany().HasForeignKey(x => x.ParentCostCenterId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<FinancialTransaction>(e =>
        {
            e.ToTable("FinancialTransactions");
            e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<BusinessPartner>().WithMany().HasForeignKey(x => x.PartnerId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<FinancialAccount>().WithMany().HasForeignKey(x => x.FinancialAccountId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<CostCenter>().WithMany().HasForeignKey(x => x.CostCenterId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<FinancialTransactionLine>(e =>
        {
            e.ToTable("FinancialTransactionLines");
            e.HasOne<FinancialTransaction>().WithMany().HasForeignKey(x => x.FinancialTransactionId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<CostCenter>().WithMany().HasForeignKey(x => x.CostCenterId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<Payable>(e =>
        {
            e.ToTable("Payables");
            e.HasOne<BusinessPartner>().WithMany().HasForeignKey(x => x.PartnerId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<Receivable>(e =>
        {
            e.ToTable("Receivables");
            e.HasOne<BusinessPartner>().WithMany().HasForeignKey(x => x.PartnerId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<Payment>(e =>
        {
            e.ToTable("Payments");
            e.HasIndex(x => x.PaymentNo).IsUnique();
            e.HasOne<BusinessPartner>().WithMany().HasForeignKey(x => x.PartnerId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<FinancialAccount>().WithMany().HasForeignKey(x => x.FinancialAccountId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<PaymentAllocation>(e =>
        {
            e.ToTable("PaymentAllocations");
            e.HasOne<Payment>().WithMany().HasForeignKey(x => x.PaymentId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<Payable>().WithMany().HasForeignKey(x => x.PayableId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<Collection>(e =>
        {
            e.ToTable("Collections");
            e.HasIndex(x => x.CollectionNo).IsUnique();
            e.HasOne<BusinessPartner>().WithMany().HasForeignKey(x => x.PartnerId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<FinancialAccount>().WithMany().HasForeignKey(x => x.FinancialAccountId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<CollectionAllocation>(e =>
        {
            e.ToTable("CollectionAllocations");
            e.HasOne<Collection>().WithMany().HasForeignKey(x => x.CollectionId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<Receivable>().WithMany().HasForeignKey(x => x.ReceivableId).OnDelete(DeleteBehavior.Restrict);
        });

        // ---- Budget ----
        b.Entity<Budget>(e =>
        {
            e.ToTable("Budgets");
            e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<CostCenter>().WithMany().HasForeignKey(x => x.CostCenterId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<BudgetLine>(e =>
        {
            e.ToTable("BudgetLines");
            e.HasOne<Budget>().WithMany().HasForeignKey(x => x.BudgetId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<CostCenter>().WithMany().HasForeignKey(x => x.CostCenterId).OnDelete(DeleteBehavior.Restrict);
        });

        // ---- Contracts ----
        b.Entity<Contract>(e =>
        {
            e.ToTable("Contracts");
            e.HasIndex(x => x.ContractNo).IsUnique();
            e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<ContractParty>(e =>
        {
            e.ToTable("ContractParties");
            e.HasOne<Contract>().WithMany().HasForeignKey(x => x.ContractId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<BusinessPartner>().WithMany().HasForeignKey(x => x.BusinessPartnerId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<ContractLine>(e =>
        {
            e.ToTable("ContractLines");
            e.HasOne<Contract>().WithMany().HasForeignKey(x => x.ContractId).OnDelete(DeleteBehavior.Cascade);
        });

        b.Entity<ContractAmendment>(e =>
        {
            e.ToTable("ContractAmendments");
            e.HasOne<Contract>().WithMany().HasForeignKey(x => x.ContractId).OnDelete(DeleteBehavior.Cascade);
        });

        // ---- ProgressPayments ----
        b.Entity<ProgressPayment>(e =>
        {
            e.ToTable("ProgressPayments");
            e.HasIndex(x => x.ProgressPaymentNo).IsUnique();
            e.HasOne<Contract>().WithMany().HasForeignKey(x => x.ContractId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<BusinessPartner>().WithMany().HasForeignKey(x => x.PartnerId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<ProgressPaymentLine>(e =>
        {
            e.ToTable("ProgressPaymentLines");
            e.HasOne<ProgressPayment>().WithMany().HasForeignKey(x => x.ProgressPaymentId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<ContractLine>().WithMany().HasForeignKey(x => x.ContractLineId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<MeasurementSheetLine>().WithMany().HasForeignKey(x => x.MeasurementSheetLineId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<ProgressPaymentDeduction>(e =>
        {
            e.ToTable("ProgressPaymentDeductions");
            e.HasOne<ProgressPayment>().WithMany().HasForeignKey(x => x.ProgressPaymentId).OnDelete(DeleteBehavior.Cascade);
        });
    }
}

