using Energy.Domain.Core;
using Energy.Domain.Identity;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Enterprise;

/// <summary>Core modülü EF Core yapılandırması (Relationship Catalogue'a göre FK ve delete behavior).</summary>
public static class CoreModuleConfiguration
{
    public static void Configure(ModelBuilder b)
    {
        b.Entity<Company>(e =>
        {
            e.ToTable("Companies");
            e.HasIndex(x => x.Code).IsUnique();
            e.HasOne<Currency>().WithMany().HasForeignKey(x => x.BaseCurrencyId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<Branch>(e =>
        {
            e.ToTable("Branches");
            e.HasIndex(x => new { x.CompanyId, x.Code }).IsUnique();
            e.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<Department>(e =>
        {
            e.ToTable("Departments");
            e.HasIndex(x => new { x.CompanyId, x.Code }).IsUnique();
            e.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Department>().WithMany().HasForeignKey(x => x.ParentDepartmentId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<User>().WithMany().HasForeignKey(x => x.ManagerUserId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<Currency>(e =>
        {
            e.ToTable("Currencies");
            e.HasIndex(x => x.Code).IsUnique();
        });

        b.Entity<ExchangeRate>(e =>
        {
            e.ToTable("ExchangeRates");
            e.HasIndex(x => new { x.CurrencyId, x.RateDate }).IsUnique();
            e.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<UnitOfMeasure>(e =>
        {
            e.ToTable("UnitsOfMeasure");
            e.HasIndex(x => x.Code).IsUnique();
        });

        b.Entity<UnitConversion>(e =>
        {
            e.ToTable("UnitConversions");
            e.HasOne<UnitOfMeasure>().WithMany().HasForeignKey(x => x.FromUnitOfMeasureId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<UnitOfMeasure>().WithMany().HasForeignKey(x => x.ToUnitOfMeasureId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<SequenceDefinition>(e =>
        {
            e.ToTable("SequenceDefinitions");
            e.HasIndex(x => new { x.Module, x.EntityType }).IsUnique();
        });

        b.Entity<SystemSetting>(e =>
        {
            e.ToTable("SystemSettings");
            e.HasIndex(x => x.Key).IsUnique();
        });
    }
}

