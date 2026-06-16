using Energy.Shared.Common;
using Energy.Domain.Modules.Assets;
using Energy.Domain.Modules.BusinessPartners;
using Energy.Domain.Modules.Catalog;
using Energy.Domain.Modules.Chat;
using Energy.Domain.Common;
using Energy.Domain.Modules.Contracts;
using Energy.Domain.Modules.Core;
using Energy.Domain.Modules.Documents;
using Energy.Domain.Modules.FieldOperations;
using Energy.Domain.Modules.Finance;
using Energy.Domain.Modules.HR;
using Energy.Domain.Modules.IAM;
using Energy.Domain.Modules.Inventory;
using Energy.Domain.Modules.Core;
using Energy.Domain.Modules.Notifications;
using Energy.Domain.Modules.Operations;
using Energy.Domain.Modules.Organization;
using Energy.Domain.Modules.Procurement;
using Energy.Domain.Modules.ProgressPayments;
using Energy.Domain.Modules.Projects;
using Energy.Domain.Modules.Reporting;
using Energy.Domain.Modules.Requests;
using Energy.Domain.Modules.Workflow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Energy.Infrastructure.Seeding;

/// <summary>
/// Sistemdeki <b>her</b> tabloya en az bir tutarlı demo kaydı ekleyen idempotent
/// tohumlayıcı. <see cref="EnsureSampleBusinessDataAsync"/>'in kurduğu çekirdek grafiği
/// (şirket → proje → cari → malzeme/depo → iş emri → satın alma → bütçe/onay) çapa
/// (anchor) olarak alır ve geri kalan tüm modülleri (organizasyon, katalog öznitelikleri,
/// stok belge/lot/hareket akışı, talep/teklif/mal kabul/fatura, saha operasyonları,
/// puantaj, ekipman, finans cari hesap akışı, sözleşme/hakediş, belge yönetimi, onay
/// akışı örnekleri, bildirim, rapor, sohbet ve doğrudan kullanıcı yetkisi/ayarı/denetim
/// kaydı) doldurur.
///
/// <para>
/// Amaç: yeni bir geliştiricinin/uygulayıcının her tablonun nasıl bağlandığını ve
/// gerçekçi bir veri grafiğinin nasıl göründüğünü tek bir kurulumda görebilmesi.
/// Her kayıt doğal anahtarına (kod/no) veya üst FK'sine göre korunur; yeniden çalıştırma
/// kopya üretmez.
/// </para>
/// </summary>
public sealed partial class SystemSeeder
{
    private async Task EnsureFullSampleDataAsync(CancellationToken ct)
    {
        // ---- Çapa (anchor) kayıtlar — çekirdek demo grafiğinden ----
        var currency = await _db.Currencies.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Code == "TRY", ct);
        var usd = await _db.Currencies.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Code == "USD", ct);
        var unit = await _db.UnitsOfMeasure.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Code == "Piece", ct);
        var packageUnit = await _db.UnitsOfMeasure.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Code == "Package", ct);
        var admin = await _db.Users.FirstOrDefaultAsync(u => u.UserName == "admin", ct);
        var company = await _db.Companies.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Code == "DEMO-CO", ct);
        var project = await _db.Projects.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Code == "PRJ-001", ct);
        var supplier = await _db.BusinessPartners.IgnoreQueryFilters().FirstOrDefaultAsync(b => b.Code == "SUP-001", ct);
        var category = await _db.MaterialCategories.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Code == "CAT-001", ct);
        var material1 = await _db.Materials.IgnoreQueryFilters().FirstOrDefaultAsync(m => m.Code == "MAT-001", ct);
        var material2 = await _db.Materials.IgnoreQueryFilters().FirstOrDefaultAsync(m => m.Code == "MAT-002", ct);
        var warehouse = await _db.Warehouses.IgnoreQueryFilters().FirstOrDefaultAsync(w => w.Code == "WH-001", ct);
        var workOrderType = await _db.WorkOrderTypes.IgnoreQueryFilters().FirstOrDefaultAsync(t => t.Code == "WOT-001", ct);
        var workOrder = await _db.WorkOrders.IgnoreQueryFilters().FirstOrDefaultAsync(w => w.WorkOrderNo == "WO-001", ct);
        var purchaseOrder = await _db.PurchaseOrders.IgnoreQueryFilters().FirstOrDefaultAsync(o => o.OrderNo == "PO-001", ct);

        if (currency is null || unit is null || admin is null || company is null || project is null ||
            supplier is null || category is null || material1 is null || material2 is null ||
            warehouse is null || workOrderType is null || workOrder is null || purchaseOrder is null)
        {
            _logger.LogWarning("Full sample data skipped: one or more core anchor records are missing.");
            return;
        }

        // İkincil demo kullanıcılar (rol şablonlarından gelir); yoksa admin'e düşülür.
        var secondUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == "ops.manager@energy.local", ct) ?? admin;
        var thirdUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == "basic.user@energy.local", ct) ?? admin;

        await SeedCoreExtrasAsync(company, currency, usd, unit, packageUnit, ct);
        var (department, employee) = await SeedOrganizationAsync(company, project, currency, admin, ct);
        var customer = await SeedBusinessPartnerDetailsAsync(supplier, currency, ct);
        var (projectPhase, _) = await SeedProjectDetailsAsync(project, admin, employee, ct);
        await SeedCatalogDetailsAsync(category, material1, unit, packageUnit, ct);
        var equipment = await SeedAssetsAsync(company, project, employee, warehouse, ct);
        await SeedInventoryFlowAsync(company, project, warehouse, material2, unit, currency, workOrder, ct);
        var requestLine = await SeedRequestsAsync(project, material1, unit, admin, ct);
        await SeedProcurementExtrasAsync(supplier, purchaseOrder, warehouse, material1, currency, requestLine, ct);
        await SeedOperationsDetailsAsync(workOrder, material1, employee, admin, ct);
        await SeedFieldOperationsAsync(project, projectPhase, workOrder, employee, equipment, material1, ct);
        await SeedHrAsync(employee, project, workOrder, ct);
        var (financialAccount, costCenter, payable, receivable) =
            await SeedFinanceAccountsAndOpenItemsAsync(supplier, customer, currency, ct);
        await SeedFinanceSettlementsAsync(supplier, customer, currency, financialAccount, payable, receivable, ct);
        var (contract, contractLine) = await SeedContractsAsync(project, currency, customer, ct);
        await SeedProgressPaymentsAsync(contract, contractLine, customer, ct);
        await SeedDocumentsAsync(project, admin, ct);
        await SeedWorkflowExtrasAsync(purchaseOrder, admin, secondUser, ct);
        await SeedNotificationsAsync(material1, admin, ct);
        await SeedReportingAsync(ct);
        await SeedDirectUserGrantsAndAuditAsync(admin, thirdUser, ct);
        await SeedChatAsync(admin, secondUser, ct);

        _logger.LogInformation("Full sample data: every table populated with at least one demo record.");
    }

    // =====================================================================================
    //  Core — şube, departman, kur, birim dönüşümü, sıra tanımı, sistem ayarı
    // =====================================================================================
    private async Task SeedCoreExtrasAsync(
        Company company, Currency currency, Currency? usd, UnitOfMeasure unit, UnitOfMeasure? packageUnit, CancellationToken ct)
    {
        await GetOrAddAsync(_db.Branches, b => b.Code == "BR-001", () => new Branch
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, Code = "BR-001", Name = "Merkez Şube",
            Address = "Ankara", IsActive = true,
        }, ct);

        await GetOrAddAsync(_db.Departments, d => d.Code == "DEP-001", () => new Department
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, Code = "DEP-001", Name = "Saha Operasyonları", IsActive = true,
        }, ct);

        if (usd is not null)
        {
            await GetOrAddAsync(_db.ExchangeRates, r => r.CurrencyId == usd.Id, () => new ExchangeRate
            {
                Id = Guid.NewGuid(), CurrencyId = usd.Id, RateDate = DateTime.UtcNow.Date, Rate = 32.50m,
            }, ct);
        }

        if (packageUnit is not null)
        {
            await GetOrAddAsync(_db.UnitConversions,
                c => c.FromUnitOfMeasureId == packageUnit.Id && c.ToUnitOfMeasureId == unit.Id,
                () => new UnitConversion
                {
                    Id = Guid.NewGuid(), FromUnitOfMeasureId = packageUnit.Id, ToUnitOfMeasureId = unit.Id, Factor = 12m,
                }, ct);
        }

        await GetOrAddAsync(_db.SequenceDefinitions,
            s => s.Module == "Procurement" && s.EntityType == "PurchaseOrder",
            () => new SequenceDefinition
            {
                Id = Guid.NewGuid(), Module = "Procurement", EntityType = "PurchaseOrder",
                Prefix = "PO-", Padding = 6, NextNumber = 2, Format = "{Prefix}{Number}",
            }, ct);

        await GetOrAddAsync(_db.SystemSettings, s => s.Key == "Demo.DefaultCompany", () => new SystemSetting
        {
            Id = Guid.NewGuid(), Key = "Demo.DefaultCompany", Value = company.Code, Category = "Demo",
            DescriptionKey = "SystemSettings.Demo.DefaultCompany.Description",
        }, ct);
    }

    // =====================================================================================
    //  Organization — pozisyon, yetkinlik, personel, yetkinlik ataması, izin, masraf
    // =====================================================================================
    private async Task<(Department Department, Employee Employee)> SeedOrganizationAsync(
        Company company, Project project, Currency currency, User admin, CancellationToken ct)
    {
        var department = await _db.Departments.IgnoreQueryFilters().FirstAsync(d => d.Code == "DEP-001", ct);
        var branch = await _db.Branches.IgnoreQueryFilters().FirstOrDefaultAsync(b => b.Code == "BR-001", ct);

        var position = await GetOrAddAsync(_db.EmployeePositions, p => p.Code == "POS-001", () => new EmployeePosition
        {
            Id = Guid.NewGuid(), Code = "POS-001", Name = "Saha Mühendisi", IsActive = true,
        }, ct);

        var skill = await GetOrAddAsync(_db.EmployeeSkills, s => s.Code == "SKL-001", () => new EmployeeSkill
        {
            Id = Guid.NewGuid(), Code = "SKL-001", Name = "Kaynakçılık", IsActive = true,
        }, ct);

        var employee = await GetOrAddAsync(_db.Employees, e => e.Code == "EMP-001", () => new Employee
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, BranchId = branch?.Id, DepartmentId = department.Id,
            EmployeePositionId = position.Id, UserId = admin.Id, Code = "EMP-001",
            FirstName = "Ali", LastName = "Usta", Phone = "5550000001", Email = "ali.usta@energy.local",
            HireDate = DateTime.UtcNow.AddYears(-1), IsActive = true,
        }, ct);

        await GetOrAddAsync(_db.EmployeeSkillAssignments,
            a => a.EmployeeId == employee.Id && a.EmployeeSkillId == skill.Id,
            () => new EmployeeSkillAssignment
            {
                Id = Guid.NewGuid(), EmployeeId = employee.Id, EmployeeSkillId = skill.Id, Level = 4, Note = "Sertifikalı",
            }, ct);

        await GetOrAddAsync(_db.LeaveRequests,
            l => l.EmployeeId == employee.Id && l.LeaveType == "Annual",
            () => new LeaveRequest
            {
                Id = Guid.NewGuid(), EmployeeId = employee.Id, LeaveType = "Annual",
                StartDate = DateTime.UtcNow.AddDays(10), EndDate = DateTime.UtcNow.AddDays(14), Days = 5m,
                Reason = "Yıllık izin", Status = ApprovalRequestStatus.Pending,
            }, ct);

        var expenseClaim = await GetOrAddAsync(_db.ExpenseClaims, c => c.ClaimNo == "EXP-001", () => new ExpenseClaim
        {
            Id = Guid.NewGuid(), EmployeeId = employee.Id, ProjectId = project.Id, CurrencyId = currency.Id,
            ClaimNo = "EXP-001", ClaimDate = DateTime.UtcNow.AddDays(-2), TotalAmount = 750m,
            Status = ApprovalRequestStatus.Pending,
        }, ct);

        await GetOrAddAsync(_db.ExpenseClaimLines,
            l => l.ExpenseClaimId == expenseClaim.Id,
            () => new ExpenseClaimLine
            {
                Id = Guid.NewGuid(), ExpenseClaimId = expenseClaim.Id, Description = "Yakıt gideri",
                ExpenseDate = DateTime.UtcNow.AddDays(-2), Amount = 750m, Category = "Travel",
            }, ct);

        return (department, employee);
    }

    // =====================================================================================
    //  BusinessPartners — müşteri cari + iletişim, adres, banka hesabı
    // =====================================================================================
    private async Task<BusinessPartner> SeedBusinessPartnerDetailsAsync(
        BusinessPartner supplier, Currency currency, CancellationToken ct)
    {
        var customer = await GetOrAddAsync(_db.BusinessPartners, b => b.Code == "CUS-001", () => new BusinessPartner
        {
            Id = Guid.NewGuid(), PartnerType = PartnerType.Customer, Code = "CUS-001",
            Name = "Marmara Enerji A.Ş.", TaxNumber = "1234567890", Phone = "5550000010",
            Email = "info@marmaraenerji.local", IsActive = true,
        }, ct);

        await GetOrAddAsync(_db.BusinessPartnerContacts,
            c => c.BusinessPartnerId == supplier.Id,
            () => new BusinessPartnerContact
            {
                Id = Guid.NewGuid(), BusinessPartnerId = supplier.Id, FullName = "Hasan Tedarik",
                Title = "Satış Müdürü", Phone = "5550000002", Email = "hasan@anadolumalzeme.local", IsPrimary = true,
            }, ct);

        await GetOrAddAsync(_db.BusinessPartnerAddresses,
            a => a.BusinessPartnerId == supplier.Id,
            () => new BusinessPartnerAddress
            {
                Id = Guid.NewGuid(), BusinessPartnerId = supplier.Id, AddressType = "Billing",
                AddressLine = "Organize Sanayi Bölgesi No:12", City = "Kocaeli", Country = "Türkiye",
                PostalCode = "41000", IsPrimary = true,
            }, ct);

        await GetOrAddAsync(_db.BusinessPartnerBankAccounts,
            a => a.BusinessPartnerId == supplier.Id,
            () => new BusinessPartnerBankAccount
            {
                Id = Guid.NewGuid(), BusinessPartnerId = supplier.Id, BankName = "Demo Bank",
                Branch = "Merkez", Iban = "TR000000000000000000000001", CurrencyId = currency.Id, IsPrimary = true,
            }, ct);

        return customer;
    }

    // =====================================================================================
    //  Projects — lokasyon, faz, üye, not
    // =====================================================================================
    private async Task<(ProjectPhase Phase, ProjectLocation Location)> SeedProjectDetailsAsync(
        Project project, User admin, Employee employee, CancellationToken ct)
    {
        var location = await GetOrAddAsync(_db.ProjectLocations,
            l => l.ProjectId == project.Id && l.Code == "LOC-001",
            () => new ProjectLocation
            {
                Id = Guid.NewGuid(), ProjectId = project.Id, Code = "LOC-001", Name = "A Blok",
            }, ct);

        var phase = await GetOrAddAsync(_db.ProjectPhases,
            p => p.ProjectId == project.Id && p.Code == "PH-001",
            () => new ProjectPhase
            {
                Id = Guid.NewGuid(), ProjectId = project.Id, Code = "PH-001", Name = "Kaba İnşaat",
                ProgressPercentage = 35m, PlannedStart = DateTime.UtcNow.AddMonths(-1), PlannedEnd = DateTime.UtcNow.AddMonths(2),
            }, ct);

        await GetOrAddAsync(_db.ProjectMembers,
            m => m.ProjectId == project.Id && m.UserId == admin.Id,
            () => new ProjectMember
            {
                Id = Guid.NewGuid(), ProjectId = project.Id, UserId = admin.Id, EmployeeId = employee.Id, ProjectRole = "Manager",
            }, ct);

        await GetOrAddAsync(_db.ProjectNotes,
            n => n.ProjectId == project.Id && n.Title == "Saha başlangıcı",
            () => new ProjectNote
            {
                Id = Guid.NewGuid(), ProjectId = project.Id, Title = "Saha başlangıcı", Body = "Mobilizasyon tamamlandı.",
            }, ct);

        return (phase, location);
    }

    // =====================================================================================
    //  Catalog — marka, dinamik öznitelik tanımı/seçeneği/kategori bağı/değeri, birim dönüşümü
    // =====================================================================================
    private async Task SeedCatalogDetailsAsync(
        MaterialCategory category, Material material, UnitOfMeasure unit, UnitOfMeasure? packageUnit, CancellationToken ct)
    {
        var brand = await GetOrAddAsync(_db.Brands, b => b.Code == "BRD-001", () => new Brand
        {
            Id = Guid.NewGuid(), Code = "BRD-001", Name = "Demo Marka", IsActive = true,
        }, ct);

        // Malzemeye marka bağla (yoksa).
        if (material.BrandId is null)
        {
            material.BrandId = brand.Id;
            await _db.SaveChangesAsync(ct);
        }

        var attribute = await GetOrAddAsync(_db.MaterialAttributeDefinitions, a => a.Code == "ATT-001", () => new MaterialAttributeDefinition
        {
            Id = Guid.NewGuid(), Code = "ATT-001", Name = "Renk", DataType = "Option", IsActive = true,
        }, ct);

        var option = await GetOrAddAsync(_db.MaterialAttributeOptions,
            o => o.MaterialAttributeDefinitionId == attribute.Id && o.Value == "Gri",
            () => new MaterialAttributeOption
            {
                Id = Guid.NewGuid(), MaterialAttributeDefinitionId = attribute.Id, Value = "Gri", DisplayOrder = 1,
            }, ct);

        await GetOrAddAsync(_db.MaterialCategoryAttributes,
            c => c.MaterialCategoryId == category.Id && c.MaterialAttributeDefinitionId == attribute.Id,
            () => new MaterialCategoryAttribute
            {
                Id = Guid.NewGuid(), MaterialCategoryId = category.Id, MaterialAttributeDefinitionId = attribute.Id,
                IsRequired = false, DisplayOrder = 1,
            }, ct);

        await GetOrAddAsync(_db.MaterialAttributeValues,
            v => v.MaterialId == material.Id && v.MaterialAttributeDefinitionId == attribute.Id,
            () => new MaterialAttributeValue
            {
                Id = Guid.NewGuid(), MaterialId = material.Id, MaterialAttributeDefinitionId = attribute.Id, OptionId = option.Id,
            }, ct);

        if (packageUnit is not null)
        {
            await GetOrAddAsync(_db.MaterialUnitConversions,
                c => c.MaterialId == material.Id && c.FromUnitOfMeasureId == packageUnit.Id && c.ToUnitOfMeasureId == unit.Id,
                () => new MaterialUnitConversion
                {
                    Id = Guid.NewGuid(), MaterialId = material.Id, FromUnitOfMeasureId = packageUnit.Id,
                    ToUnitOfMeasureId = unit.Id, Factor = 25m,
                }, ct);
        }
    }

    // =====================================================================================
    //  Assets — ekipman kartı, atama, bakım
    // =====================================================================================
    private async Task<EquipmentAsset> SeedAssetsAsync(
        Company company, Project project, Employee employee, Warehouse warehouse, CancellationToken ct)
    {
        var equipment = await GetOrAddAsync(_db.EquipmentAssets, e => e.Code == "EQ-001", () => new EquipmentAsset
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, Code = "EQ-001", Name = "Ekskavatör",
            AssetType = "Machine", SerialNo = "SN-EQ-001", PurchaseDate = DateTime.UtcNow.AddYears(-2), IsActive = true,
        }, ct);

        await GetOrAddAsync(_db.EquipmentAssignments,
            a => a.EquipmentAssetId == equipment.Id && a.ProjectId == project.Id,
            () => new EquipmentAssignment
            {
                Id = Guid.NewGuid(), EquipmentAssetId = equipment.Id, ProjectId = project.Id, EmployeeId = employee.Id,
                WarehouseId = warehouse.Id, StartDate = DateTime.UtcNow.AddMonths(-1), IsActive = true,
            }, ct);

        await GetOrAddAsync(_db.EquipmentMaintenances,
            m => m.EquipmentAssetId == equipment.Id,
            () => new EquipmentMaintenance
            {
                Id = Guid.NewGuid(), EquipmentAssetId = equipment.Id, MaintenanceType = "Planned",
                ScheduledDate = DateTime.UtcNow.AddDays(30), Cost = 1500m, Note = "Periyodik bakım",
            }, ct);

        return equipment;
    }

    // =====================================================================================
    //  Inventory — lokasyon, belge türleri, giriş+çıkış belgesi/satırı, lot, hareket,
    //  FIFO dağılımı, rezervasyon, sayım, depolar arası transfer
    // =====================================================================================
    private async Task SeedInventoryFlowAsync(
        Company company, Project project, Warehouse warehouse, Material material, UnitOfMeasure unit,
        Currency currency, WorkOrder workOrder, CancellationToken ct)
    {
        await GetOrAddAsync(_db.WarehouseLocations,
            l => l.WarehouseId == warehouse.Id && l.Code == "WL-001",
            () => new WarehouseLocation
            {
                Id = Guid.NewGuid(), WarehouseId = warehouse.Id, Code = "WL-001", Name = "Raf A1",
            }, ct);

        var inType = await GetOrAddAsync(_db.StockDocumentTypes, t => t.Code == "SDT-IN", () => new StockDocumentType
        {
            Id = Guid.NewGuid(), Code = "SDT-IN", Name = "Mal Girişi", Direction = "In", IsActive = true,
        }, ct);
        var outType = await GetOrAddAsync(_db.StockDocumentTypes, t => t.Code == "SDT-OUT", () => new StockDocumentType
        {
            Id = Guid.NewGuid(), Code = "SDT-OUT", Name = "Sarf Çıkışı", Direction = "Out", IsActive = true,
        }, ct);

        // Giriş belgesi + satırı + lot + (+) hareket.
        var inDoc = await GetOrAddAsync(_db.StockDocuments, d => d.DocumentNo == "SD-001", () => new StockDocument
        {
            Id = Guid.NewGuid(), DocumentTypeId = inType.Id, TargetWarehouseId = warehouse.Id, ProjectId = project.Id,
            Status = DocumentStatus.Approved, DocumentNo = "SD-001", DocumentDate = DateTime.UtcNow.AddDays(-7), Note = "İlk giriş",
        }, ct);
        var inLine = await GetOrAddAsync(_db.StockDocumentLines,
            l => l.StockDocumentId == inDoc.Id,
            () => new StockDocumentLine
            {
                Id = Guid.NewGuid(), StockDocumentId = inDoc.Id, MaterialId = material.Id, UnitOfMeasureId = unit.Id,
                Quantity = 100m, UnitPrice = 1450m, CurrencyId = currency.Id, Note = "Açılış stoğu",
            }, ct);
        var lot = await GetOrAddAsync(_db.StockLots, l => l.LotNo == "LOT-001", () => new StockLot
        {
            Id = Guid.NewGuid(), WarehouseId = warehouse.Id, MaterialId = material.Id, SourceStockDocumentLineId = inLine.Id,
            LotNo = "LOT-001", InitialQuantity = 100m, RemainingQuantity = 90m, UnitCost = 1450m, ReceivedAt = DateTime.UtcNow.AddDays(-7),
        }, ct);
        await GetOrAddAsync(_db.StockTransactions,
            t => t.StockDocumentLineId == inLine.Id,
            () => new StockTransaction
            {
                Id = Guid.NewGuid(), StockDocumentId = inDoc.Id, StockDocumentLineId = inLine.Id, StockLotId = lot.Id,
                WarehouseId = warehouse.Id, MaterialId = material.Id, Quantity = 100m, UnitCost = 1450m, TransactionDate = DateTime.UtcNow.AddDays(-7),
            }, ct);

        // Çıkış belgesi + satırı + FIFO dağılımı + (-) hareket.
        var outDoc = await GetOrAddAsync(_db.StockDocuments, d => d.DocumentNo == "SD-002", () => new StockDocument
        {
            Id = Guid.NewGuid(), DocumentTypeId = outType.Id, SourceWarehouseId = warehouse.Id, ProjectId = project.Id,
            Status = DocumentStatus.Approved, DocumentNo = "SD-002", DocumentDate = DateTime.UtcNow.AddDays(-3), Note = "Sahaya sarf",
        }, ct);
        var outLine = await GetOrAddAsync(_db.StockDocumentLines,
            l => l.StockDocumentId == outDoc.Id,
            () => new StockDocumentLine
            {
                Id = Guid.NewGuid(), StockDocumentId = outDoc.Id, MaterialId = material.Id, UnitOfMeasureId = unit.Id,
                Quantity = 10m, UnitPrice = 1450m, CurrencyId = currency.Id, Note = "Sarf",
            }, ct);
        await GetOrAddAsync(_db.StockIssueAllocations,
            a => a.StockDocumentLineId == outLine.Id,
            () => new StockIssueAllocation
            {
                Id = Guid.NewGuid(), StockDocumentLineId = outLine.Id, StockLotId = lot.Id, Quantity = 10m, UnitCost = 1450m,
            }, ct);
        await GetOrAddAsync(_db.StockTransactions,
            t => t.StockDocumentLineId == outLine.Id,
            () => new StockTransaction
            {
                Id = Guid.NewGuid(), StockDocumentId = outDoc.Id, StockDocumentLineId = outLine.Id, StockLotId = lot.Id,
                WarehouseId = warehouse.Id, MaterialId = material.Id, Quantity = -10m, UnitCost = 1450m, TransactionDate = DateTime.UtcNow.AddDays(-3),
            }, ct);

        // Rezervasyon (iş emrine).
        await GetOrAddAsync(_db.StockReservations,
            r => r.MaterialId == material.Id && r.RelatedEntityId == workOrder.Id,
            () => new StockReservation
            {
                Id = Guid.NewGuid(), WarehouseId = warehouse.Id, MaterialId = material.Id, Quantity = 5m,
                RelatedModule = "Operations", RelatedEntityType = "WorkOrder", RelatedEntityId = workOrder.Id, IsReleased = false,
            }, ct);

        // Sayım başlığı + satırı.
        var count = await GetOrAddAsync(_db.StockCounts, c => c.CountNo == "SC-001", () => new StockCount
        {
            Id = Guid.NewGuid(), WarehouseId = warehouse.Id, CountNo = "SC-001", CountDate = DateTime.UtcNow.AddDays(-1),
            Status = DocumentStatus.Approved,
        }, ct);
        await GetOrAddAsync(_db.StockCountLines,
            l => l.StockCountId == count.Id,
            () => new StockCountLine
            {
                Id = Guid.NewGuid(), StockCountId = count.Id, MaterialId = material.Id, SystemQuantity = 90m, CountedQuantity = 89m,
            }, ct);

        // Depolar arası transfer (ikinci depo gerekli).
        var warehouse2 = await GetOrAddAsync(_db.Warehouses, w => w.Code == "WH-002", () => new Warehouse
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, ProjectId = project.Id, WarehouseType = WarehouseType.ProjectSite,
            Code = "WH-002", Name = "Saha Deposu", IsActive = true,
        }, ct);
        var transfer = await GetOrAddAsync(_db.WarehouseTransfers, t => t.TransferNo == "WT-001", () => new WarehouseTransfer
        {
            Id = Guid.NewGuid(), SourceWarehouseId = warehouse.Id, TargetWarehouseId = warehouse2.Id,
            TransferNo = "WT-001", TransferDate = DateTime.UtcNow.AddDays(-2), Status = DocumentStatus.Approved,
        }, ct);
        await GetOrAddAsync(_db.WarehouseTransferLines,
            l => l.WarehouseTransferId == transfer.Id,
            () => new WarehouseTransferLine
            {
                Id = Guid.NewGuid(), WarehouseTransferId = transfer.Id, MaterialId = material.Id, Quantity = 20m,
            }, ct);
    }

    // =====================================================================================
    //  Requests — talep türü, talep başlığı, talep satırı
    // =====================================================================================
    private async Task<RequestLine> SeedRequestsAsync(
        Project project, Material material, UnitOfMeasure unit, User admin, CancellationToken ct)
    {
        var requestType = await GetOrAddAsync(_db.RequestTypes, t => t.Code == "RQT-001", () => new RequestType
        {
            Id = Guid.NewGuid(), Code = "RQT-001", Name = "Malzeme Talebi", Category = "Material", IsActive = true,
        }, ct);

        var request = await GetOrAddAsync(_db.Requests, r => r.RequestNo == "REQ-001", () => new Request
        {
            Id = Guid.NewGuid(), RequestTypeId = requestType.Id, ProjectId = project.Id, RequestedByUserId = admin.Id,
            Status = RequestStatus.Approved, RequestNo = "REQ-001", RequestDate = DateTime.UtcNow.AddDays(-6),
            Description = "Saha malzeme ihtiyacı",
        }, ct);

        return await GetOrAddAsync(_db.RequestLines,
            l => l.RequestId == request.Id,
            () => new RequestLine
            {
                Id = Guid.NewGuid(), RequestId = request.Id, MaterialId = material.Id, Quantity = 50m,
                UnitOfMeasureId = unit.Id, Note = "Acil",
            }, ct);
    }

    // =====================================================================================
    //  Procurement — teklif+satır, mal kabul+satır, tedarikçi faturası+satır
    // =====================================================================================
    private async Task SeedProcurementExtrasAsync(
        BusinessPartner supplier, PurchaseOrder purchaseOrder, Warehouse warehouse, Material material,
        Currency currency, RequestLine requestLine, CancellationToken ct)
    {
        var quote = await GetOrAddAsync(_db.SupplierQuotes, q => q.QuoteNo == "SQ-001", () => new SupplierQuote
        {
            Id = Guid.NewGuid(), SupplierId = supplier.Id, CurrencyId = currency.Id, QuoteNo = "SQ-001",
            QuoteDate = DateTime.UtcNow.AddDays(-5), PaymentTerm = "30 gün", Status = DocumentStatus.Approved, IsSelected = true,
        }, ct);
        await GetOrAddAsync(_db.SupplierQuoteLines,
            l => l.SupplierQuoteId == quote.Id,
            () => new SupplierQuoteLine
            {
                Id = Guid.NewGuid(), SupplierQuoteId = quote.Id, RequestLineId = requestLine.Id, MaterialId = material.Id,
                Description = "Teklif kalemi", Quantity = 50m, UnitPrice = 118m, TaxRate = 20m, DiscountRate = 5m, DeliveryDays = 7,
            }, ct);

        var receipt = await GetOrAddAsync(_db.PurchaseReceipts, r => r.ReceiptNo == "PR-001", () => new PurchaseReceipt
        {
            Id = Guid.NewGuid(), SupplierId = supplier.Id, PurchaseOrderId = purchaseOrder.Id, WarehouseId = warehouse.Id,
            ReceiptNo = "PR-001", ReceiptDate = DateTime.UtcNow.AddDays(-1), Status = DocumentStatus.Approved,
        }, ct);
        await GetOrAddAsync(_db.PurchaseReceiptLines,
            l => l.PurchaseReceiptId == receipt.Id,
            () => new PurchaseReceiptLine
            {
                Id = Guid.NewGuid(), PurchaseReceiptId = receipt.Id, MaterialId = material.Id, Quantity = 30m, UnitPrice = 120m,
            }, ct);

        var invoice = await GetOrAddAsync(_db.SupplierInvoices, i => i.InvoiceNo == "SI-001", () => new SupplierInvoice
        {
            Id = Guid.NewGuid(), SupplierId = supplier.Id, PurchaseOrderId = purchaseOrder.Id, PurchaseReceiptId = receipt.Id,
            CurrencyId = currency.Id, InvoiceNo = "SI-001", InvoiceDate = DateTime.UtcNow, TotalAmount = 4320m, Status = DocumentStatus.Approved,
        }, ct);
        await GetOrAddAsync(_db.SupplierInvoiceLines,
            l => l.SupplierInvoiceId == invoice.Id,
            () => new SupplierInvoiceLine
            {
                Id = Guid.NewGuid(), SupplierInvoiceId = invoice.Id, MaterialId = material.Id,
                Description = "Fatura kalemi", Quantity = 30m, UnitPrice = 120m, TaxRate = 20m,
            }, ct);
    }

    // =====================================================================================
    //  Operations — atama, malzeme planı/kullanımı, kontrol listesi+satırı, durum geçmişi
    // =====================================================================================
    private async Task SeedOperationsDetailsAsync(
        WorkOrder workOrder, Material material, Employee employee, User admin, CancellationToken ct)
    {
        await GetOrAddAsync(_db.WorkOrderAssignments,
            a => a.WorkOrderId == workOrder.Id && a.EmployeeId == employee.Id,
            () => new WorkOrderAssignment
            {
                Id = Guid.NewGuid(), WorkOrderId = workOrder.Id, EmployeeId = employee.Id, UserId = admin.Id, AssignmentRole = "Lead",
            }, ct);

        await GetOrAddAsync(_db.WorkOrderMaterialPlans,
            p => p.WorkOrderId == workOrder.Id && p.MaterialId == material.Id,
            () => new WorkOrderMaterialPlan
            {
                Id = Guid.NewGuid(), WorkOrderId = workOrder.Id, MaterialId = material.Id, PlannedQuantity = 40m,
            }, ct);

        await GetOrAddAsync(_db.WorkOrderMaterialUsages,
            u => u.WorkOrderId == workOrder.Id && u.MaterialId == material.Id,
            () => new WorkOrderMaterialUsage
            {
                Id = Guid.NewGuid(), WorkOrderId = workOrder.Id, MaterialId = material.Id, UsedQuantity = 10m,
            }, ct);

        var checklist = await GetOrAddAsync(_db.WorkOrderChecklists,
            c => c.WorkOrderId == workOrder.Id,
            () => new WorkOrderChecklist
            {
                Id = Guid.NewGuid(), WorkOrderId = workOrder.Id, Name = "İSG Kontrolleri", IsRequired = true,
            }, ct);
        await GetOrAddAsync(_db.WorkOrderChecklistItems,
            i => i.WorkOrderChecklistId == checklist.Id,
            () => new WorkOrderChecklistItem
            {
                Id = Guid.NewGuid(), WorkOrderChecklistId = checklist.Id, Description = "Baret takıldı mı?",
                IsRequired = true, IsCompleted = true,
            }, ct);

        await GetOrAddAsync(_db.WorkOrderStatusHistories,
            h => h.WorkOrderId == workOrder.Id,
            () => new WorkOrderStatusHistory
            {
                Id = Guid.NewGuid(), WorkOrderId = workOrder.Id, FromStatus = WorkOrderStatus.Draft,
                ToStatus = WorkOrderStatus.InProgress, ChangedAt = DateTime.UtcNow.AddDays(-5), Note = "Çalışma başladı",
            }, ct);
    }

    // =====================================================================================
    //  FieldOperations — günlük saha raporu (+işçi/ekipman/malzeme), ilerleme, metraj
    // =====================================================================================
    private async Task SeedFieldOperationsAsync(
        Project project, ProjectPhase phase, WorkOrder workOrder, Employee employee, EquipmentAsset equipment,
        Material material, CancellationToken ct)
    {
        var report = await GetOrAddAsync(_db.DailySiteReports, r => r.ReportNo == "DSR-001", () => new DailySiteReport
        {
            Id = Guid.NewGuid(), ProjectId = project.Id, WorkOrderId = workOrder.Id, ReportNo = "DSR-001",
            ReportDate = DateTime.UtcNow.AddDays(-1), Weather = "Açık", Notes = "Çalışma sorunsuz", Status = DocumentStatus.Approved,
        }, ct);
        await GetOrAddAsync(_db.DailySiteReportWorkers,
            w => w.DailySiteReportId == report.Id,
            () => new DailySiteReportWorker
            {
                Id = Guid.NewGuid(), DailySiteReportId = report.Id, EmployeeId = employee.Id, HoursWorked = 8m, Note = "Tam gün",
            }, ct);
        await GetOrAddAsync(_db.DailySiteReportEquipments,
            e => e.DailySiteReportId == report.Id,
            () => new DailySiteReportEquipment
            {
                Id = Guid.NewGuid(), DailySiteReportId = report.Id, EquipmentAssetId = equipment.Id, Hours = 6m,
            }, ct);
        await GetOrAddAsync(_db.DailySiteReportMaterials,
            m => m.DailySiteReportId == report.Id,
            () => new DailySiteReportMaterial
            {
                Id = Guid.NewGuid(), DailySiteReportId = report.Id, MaterialId = material.Id, Quantity = 10m,
            }, ct);

        await GetOrAddAsync(_db.ProgressEntries,
            p => p.ProjectId == project.Id && p.ProjectPhaseId == phase.Id,
            () => new ProgressEntry
            {
                Id = Guid.NewGuid(), ProjectId = project.Id, ProjectPhaseId = phase.Id, EntryDate = DateTime.UtcNow.AddDays(-1),
                Quantity = 120m, Percentage = 35m, Note = "Kaba inşaat ilerlemesi",
            }, ct);

        var sheet = await GetOrAddAsync(_db.MeasurementSheets, s => s.SheetNo == "MS-001", () => new MeasurementSheet
        {
            Id = Guid.NewGuid(), ProjectId = project.Id, SheetNo = "MS-001", SheetDate = DateTime.UtcNow.AddDays(-1), Status = DocumentStatus.Approved,
        }, ct);
        await GetOrAddAsync(_db.MeasurementSheetLines,
            l => l.MeasurementSheetId == sheet.Id,
            () => new MeasurementSheetLine
            {
                Id = Guid.NewGuid(), MeasurementSheetId = sheet.Id, Description = "Beton dökümü", Quantity = 120m, UnitPrice = 950m,
            }, ct);
    }

    // =====================================================================================
    //  HR — puantaj başlığı + satırı
    // =====================================================================================
    private async Task SeedHrAsync(Employee employee, Project project, WorkOrder workOrder, CancellationToken ct)
    {
        var timesheet = await GetOrAddAsync(_db.Timesheets, t => t.TimesheetNo == "TS-001", () => new Timesheet
        {
            Id = Guid.NewGuid(), TimesheetNo = "TS-001", PeriodStart = DateTime.UtcNow.AddDays(-7), PeriodEnd = DateTime.UtcNow,
            Status = ApprovalRequestStatus.Pending,
        }, ct);
        await GetOrAddAsync(_db.TimesheetLines,
            l => l.TimesheetId == timesheet.Id,
            () => new TimesheetLine
            {
                Id = Guid.NewGuid(), TimesheetId = timesheet.Id, EmployeeId = employee.Id, ProjectId = project.Id,
                WorkOrderId = workOrder.Id, WorkDate = DateTime.UtcNow.AddDays(-1), NormalHours = 8m, OvertimeHours = 2m, HourlyCost = 150m,
            }, ct);
    }

    // =====================================================================================
    //  Finance — hesap, maliyet merkezi, borç/alacak açık kalemleri
    // =====================================================================================
    private async Task<(FinancialAccount Account, CostCenter CostCenter, Payable Payable, Receivable Receivable)>
        SeedFinanceAccountsAndOpenItemsAsync(BusinessPartner supplier, BusinessPartner customer, Currency currency, CancellationToken ct)
    {
        var account = await GetOrAddAsync(_db.FinancialAccounts, a => a.Code == "FA-001", () => new FinancialAccount
        {
            Id = Guid.NewGuid(), Code = "FA-001", Name = "Merkez Banka Hesabı", AccountType = "Bank", CurrencyId = currency.Id, IsActive = true,
        }, ct);

        var costCenter = await GetOrAddAsync(_db.CostCenters, c => c.Code == "CC-001", () => new CostCenter
        {
            Id = Guid.NewGuid(), Code = "CC-001", Name = "Saha Maliyet Merkezi", IsActive = true,
        }, ct);

        var payable = await GetOrAddAsync(_db.Payables,
            p => p.PartnerId == supplier.Id && !p.IsClosed,
            () => new Payable
            {
                Id = Guid.NewGuid(), PartnerId = supplier.Id, CurrencyId = currency.Id, Amount = 4320m, RemainingAmount = 4320m,
                DueDate = DateTime.UtcNow.AddDays(20), RelatedModule = "Procurement", RelatedEntityType = "SupplierInvoice", IsClosed = false,
            }, ct);

        var receivable = await GetOrAddAsync(_db.Receivables,
            r => r.PartnerId == customer.Id && !r.IsClosed,
            () => new Receivable
            {
                Id = Guid.NewGuid(), PartnerId = customer.Id, CurrencyId = currency.Id, Amount = 50000m, RemainingAmount = 50000m,
                DueDate = DateTime.UtcNow.AddDays(15), RelatedModule = "ProgressPayments", RelatedEntityType = "ProgressPayment", IsClosed = false,
            }, ct);

        return (account, costCenter, payable, receivable);
    }

    // =====================================================================================
    //  Finance — ödeme + borç dağılımı, tahsilat + alacak dağılımı
    // =====================================================================================
    private async Task SeedFinanceSettlementsAsync(
        BusinessPartner supplier, BusinessPartner customer, Currency currency, FinancialAccount account,
        Payable payable, Receivable receivable, CancellationToken ct)
    {
        var payment = await GetOrAddAsync(_db.Payments, p => p.PaymentNo == "PAY-001", () => new Payment
        {
            Id = Guid.NewGuid(), PartnerId = supplier.Id, CurrencyId = currency.Id, FinancialAccountId = account.Id,
            Amount = 2000m, PaymentDate = DateTime.UtcNow.AddDays(-1), PaymentNo = "PAY-001", Status = ApprovalRequestStatus.Approved,
        }, ct);
        await GetOrAddAsync(_db.PaymentAllocations,
            a => a.PaymentId == payment.Id,
            () => new PaymentAllocation { Id = Guid.NewGuid(), PaymentId = payment.Id, PayableId = payable.Id, Amount = 2000m }, ct);

        var collection = await GetOrAddAsync(_db.Collections, c => c.CollectionNo == "COL-001", () => new Collection
        {
            Id = Guid.NewGuid(), PartnerId = customer.Id, CurrencyId = currency.Id, FinancialAccountId = account.Id,
            Amount = 15000m, CollectionDate = DateTime.UtcNow.AddDays(-1), CollectionNo = "COL-001", Status = ApprovalRequestStatus.Approved,
        }, ct);
        await GetOrAddAsync(_db.CollectionAllocations,
            a => a.CollectionId == collection.Id,
            () => new CollectionAllocation { Id = Guid.NewGuid(), CollectionId = collection.Id, ReceivableId = receivable.Id, Amount = 15000m }, ct);
    }

    // =====================================================================================
    //  Contracts — sözleşme, taraf, kalem, ek protokol
    // =====================================================================================
    private async Task<(Contract Contract, ContractLine Line)> SeedContractsAsync(
        Project project, Currency currency, BusinessPartner customer, CancellationToken ct)
    {
        var contract = await GetOrAddAsync(_db.Contracts, c => c.ContractNo == "CON-001", () => new Contract
        {
            Id = Guid.NewGuid(), ContractType = ContractType.Customer, ProjectId = project.Id, CurrencyId = currency.Id,
            ContractNo = "CON-001", Title = "Ana Yüklenici Sözleşmesi", ContractAmount = 1000000m,
            StartDate = DateTime.UtcNow.AddMonths(-2), EndDate = DateTime.UtcNow.AddMonths(10), Status = DocumentStatus.Approved,
        }, ct);

        await GetOrAddAsync(_db.ContractParties,
            p => p.ContractId == contract.Id && p.BusinessPartnerId == customer.Id,
            () => new ContractParty
            {
                Id = Guid.NewGuid(), ContractId = contract.Id, BusinessPartnerId = customer.Id, PartyRole = "Customer",
            }, ct);

        var line = await GetOrAddAsync(_db.ContractLines,
            l => l.ContractId == contract.Id,
            () => new ContractLine
            {
                Id = Guid.NewGuid(), ContractId = contract.Id, Description = "Kaba inşaat işleri", Quantity = 1m, UnitPrice = 1000000m,
            }, ct);

        await GetOrAddAsync(_db.ContractAmendments,
            a => a.ContractId == contract.Id,
            () => new ContractAmendment
            {
                Id = Guid.NewGuid(), ContractId = contract.Id, AmendmentNo = "CA-001", AmendmentDate = DateTime.UtcNow.AddDays(-10),
                Description = "Kapsam genişletme", AmountDelta = 50000m,
            }, ct);

        return (contract, line);
    }

    // =====================================================================================
    //  ProgressPayments — hakediş başlığı, satırı, kesintisi
    // =====================================================================================
    private async Task SeedProgressPaymentsAsync(
        Contract contract, ContractLine contractLine, BusinessPartner customer, CancellationToken ct)
    {
        var pp = await GetOrAddAsync(_db.ProgressPayments, p => p.ProgressPaymentNo == "PP-001", () => new ProgressPayment
        {
            Id = Guid.NewGuid(), ContractId = contract.Id, PartnerId = customer.Id, ProgressPaymentNo = "PP-001",
            PaymentPeriodStart = DateTime.UtcNow.AddMonths(-1), PaymentPeriodEnd = DateTime.UtcNow,
            GrossAmount = 200000m, DeductionTotal = 20000m, NetAmount = 180000m, Status = ApprovalRequestStatus.Pending,
        }, ct);

        await GetOrAddAsync(_db.ProgressPaymentLines,
            l => l.ProgressPaymentId == pp.Id,
            () => new ProgressPaymentLine
            {
                Id = Guid.NewGuid(), ProgressPaymentId = pp.Id, ContractLineId = contractLine.Id,
                Description = "Dönem imalatı", Quantity = 0.2m, UnitPrice = 1000000m, Amount = 200000m,
            }, ct);

        await GetOrAddAsync(_db.ProgressPaymentDeductions,
            d => d.ProgressPaymentId == pp.Id,
            () => new ProgressPaymentDeduction
            {
                Id = Guid.NewGuid(), ProgressPaymentId = pp.Id, DeductionType = "Retention", Amount = 20000m, Note = "Teminat kesintisi",
            }, ct);
    }

    // =====================================================================================
    //  Documents — klasör, belge, versiyon, ilişki, erişim yetkisi
    // =====================================================================================
    private async Task SeedDocumentsAsync(Project project, User admin, CancellationToken ct)
    {
        var folder = await GetOrAddAsync(_db.DocumentFolders, f => f.Name == "Proje Belgeleri", () => new DocumentFolder
        {
            Id = Guid.NewGuid(), Name = "Proje Belgeleri",
        }, ct);

        var document = await GetOrAddAsync(_db.Documents,
            d => d.Name == "Sözleşme PDF" && d.DocumentFolderId == folder.Id,
            () => new Document
            {
                Id = Guid.NewGuid(), DocumentFolderId = folder.Id, Name = "Sözleşme PDF", Description = "Ana sözleşme",
                Status = DocumentStatus.Approved, CurrentVersionNo = 1,
            }, ct);

        await GetOrAddAsync(_db.DocumentVersions,
            v => v.DocumentId == document.Id && v.VersionNo == 1,
            () => new DocumentVersion
            {
                Id = Guid.NewGuid(), DocumentId = document.Id, VersionNo = 1, FileName = "sozlesme.pdf",
                FilePath = "/demo/sozlesme.pdf", FileSize = 102400, ContentType = "application/pdf", UploadedAt = DateTime.UtcNow.AddDays(-3),
            }, ct);

        await GetOrAddAsync(_db.DocumentRelations,
            r => r.DocumentId == document.Id && r.RelatedEntityId == project.Id,
            () => new DocumentRelation
            {
                Id = Guid.NewGuid(), DocumentId = document.Id, RelatedModule = "Projects", RelatedEntityType = "Project", RelatedEntityId = project.Id,
            }, ct);

        await GetOrAddAsync(_db.DocumentPermissions,
            p => p.DocumentId == document.Id && p.UserId == admin.Id,
            () => new DocumentPermission
            {
                Id = Guid.NewGuid(), DocumentId = document.Id, UserId = admin.Id, AccessType = "Manage",
            }, ct);
    }

    // =====================================================================================
    //  Workflow — onay koşulu, talep adımı/onaycısı, onay hareketi, yetki devri
    // =====================================================================================
    private async Task SeedWorkflowExtrasAsync(PurchaseOrder purchaseOrder, User admin, User secondUser, CancellationToken ct)
    {
        var version = await (from v in _db.ApprovalDefinitionVersions.IgnoreQueryFilters()
                             join d in _db.ApprovalDefinitions.IgnoreQueryFilters() on v.ApprovalDefinitionId equals d.Id
                             where d.Code == "PurchaseOrderApproval" && v.IsActive
                             select v).FirstOrDefaultAsync(ct);
        if (version is not null)
        {
            await GetOrAddAsync(_db.ApprovalConditions,
                c => c.ApprovalDefinitionVersionId == version.Id && c.FieldName == "Amount",
                () => new ApprovalCondition
                {
                    Id = Guid.NewGuid(), ApprovalDefinitionVersionId = version.Id, FieldName = "Amount",
                    Operator = ConditionOperator.GreaterThanOrEqual, ValueNumber = 1000m,
                }, ct);
        }

        var request = await _db.ApprovalRequests.IgnoreQueryFilters()
            .FirstOrDefaultAsync(a => a.RelatedModule == "Procurement" && a.RelatedEntityId == purchaseOrder.Id, ct);
        if (request is not null)
        {
            var stepDef = await (from s in _db.ApprovalStepDefinitions.IgnoreQueryFilters()
                                 where s.ApprovalDefinitionVersionId == request.ApprovalDefinitionVersionId && s.StepNo == 1
                                 select s).FirstOrDefaultAsync(ct);
            if (stepDef is not null)
            {
                var requestStep = await GetOrAddAsync(_db.ApprovalRequestSteps,
                    s => s.ApprovalRequestId == request.Id && s.StepNo == 1,
                    () => new ApprovalRequestStep
                    {
                        Id = Guid.NewGuid(), ApprovalRequestId = request.Id, ApprovalStepDefinitionId = stepDef.Id,
                        StepNo = 1, ApprovalMode = ApprovalMode.Sequential, Status = ApprovalStepStatus.Active,
                    }, ct);

                await GetOrAddAsync(_db.ApprovalRequestApprovers,
                    a => a.ApprovalRequestStepId == requestStep.Id && a.UserId == admin.Id,
                    () => new ApprovalRequestApprover
                    {
                        Id = Guid.NewGuid(), ApprovalRequestStepId = requestStep.Id, UserId = admin.Id, Status = ApprovalApproverStatus.Waiting,
                    }, ct);

                await GetOrAddAsync(_db.ApprovalActions,
                    x => x.ApprovalRequestId == request.Id,
                    () => new ApprovalAction
                    {
                        Id = Guid.NewGuid(), ApprovalRequestId = request.Id, ApprovalRequestStepId = requestStep.Id,
                        UserId = admin.Id, ActionType = ApprovalActionType.Return, ActionAt = DateTime.UtcNow.AddHours(-2),
                        Note = "Ek bilgi istendi (demo hareketi)",
                    }, ct);
            }
        }

        if (secondUser.Id != admin.Id)
        {
            await GetOrAddAsync(_db.ApprovalDelegations,
                d => d.DelegatorUserId == admin.Id && d.DelegateUserId == secondUser.Id,
                () => new ApprovalDelegation
                {
                    Id = Guid.NewGuid(), DelegatorUserId = admin.Id, DelegateUserId = secondUser.Id,
                    StartDate = DateTime.UtcNow.AddDays(-1), EndDate = DateTime.UtcNow.AddDays(7), IsActive = true,
                }, ct);
        }
    }

    // =====================================================================================
    //  Notifications — bildirim, alıcı, tercih
    // =====================================================================================
    private async Task SeedNotificationsAsync(Material material, User admin, CancellationToken ct)
    {
        var notification = await GetOrAddAsync(_db.Notifications,
            n => n.NotificationType == "LowStock" && n.RelatedEntityId == material.Id,
            () => new Notification
            {
                Id = Guid.NewGuid(), Title = "Düşük stok uyarısı", Body = "Çimento 50kg stoğu kritik seviyede.",
                NotificationType = "LowStock", RelatedModule = "Inventory", RelatedEntityType = "Material", RelatedEntityId = material.Id,
            }, ct);

        await GetOrAddAsync(_db.NotificationRecipients,
            r => r.NotificationId == notification.Id && r.UserId == admin.Id,
            () => new NotificationRecipient
            {
                Id = Guid.NewGuid(), NotificationId = notification.Id, UserId = admin.Id, IsRead = false,
            }, ct);

        await GetOrAddAsync(_db.NotificationPreferences,
            p => p.UserId == admin.Id && p.NotificationType == "LowStock",
            () => new NotificationPreference
            {
                Id = Guid.NewGuid(), UserId = admin.Id, NotificationType = "LowStock", InAppEnabled = true, EmailEnabled = true,
            }, ct);
    }

    // =====================================================================================
    //  Reporting — rapor tanımı (DashboardWidget zaten kurumsal tohumlamada eklenir)
    // =====================================================================================
    private async Task SeedReportingAsync(CancellationToken ct)
    {
        await GetOrAddAsync(_db.ReportDefinitions, r => r.Code == "RPT-001", () => new ReportDefinition
        {
            Id = Guid.NewGuid(), Code = "RPT-001", Name = "Proje Maliyet Raporu", Module = "Reporting",
            QueryKey = "project-cost-summary", RequiredPermissionCode = "Reporting.ReadAll", IsActive = true,
        }, ct);
    }

    // =====================================================================================
    //  IAM ekleri — doğrudan kullanıcı yetkisi, kullanıcı ayarı, denetim kaydı
    // =====================================================================================
    private async Task SeedDirectUserGrantsAndAuditAsync(User admin, User thirdUser, CancellationToken ct)
    {
        // Doğrudan kullanıcı→yetki ataması: rolü üzerinden gelmeyen ek bir yetki.
        if (!await _db.UserPermissions.AnyAsync(up => up.UserId == thirdUser.Id && up.PermissionCode == "Reporting.Export", ct))
        {
            _db.UserPermissions.Add(new UserPermission { UserId = thirdUser.Id, PermissionCode = "Reporting.Export" });
            await _db.SaveChangesAsync(ct);
        }

        // Kullanıcı tercihi (her kullanıcı için tek satır, UserId ile anahtarlı).
        if (!await _db.UserSettings.AnyAsync(s => s.UserId == admin.Id, ct))
        {
            _db.UserSettings.Add(new UserSetting
            {
                UserId = admin.Id, NotificationSound = true, CallSound = true, DesktopNotifications = true,
                ReadReceipts = true, Theme = "system", UpdatedAt = DateTime.UtcNow,
            });
            await _db.SaveChangesAsync(ct);
        }

        // Denetim kaydı (append-only) — örnek bir başarılı istek.
        if (!await _db.AuditLogs.AnyAsync(a => a.Path == "/seed/demo", ct))
        {
            _db.AuditLogs.Add(new AuditLog
            {
                OccurredAt = DateTime.UtcNow, UserId = admin.Id, UserName = "admin", IpAddress = "127.0.0.1",
                HttpMethod = "GET", Path = "/seed/demo", StatusCode = 200, IsSuccess = true, Source = "Seed",
                CorrelationId = Guid.NewGuid(), DurationMs = 5,
            });
            await _db.SaveChangesAsync(ct);
        }
    }

    // =====================================================================================
    //  Chat — grup, üyeler, birebir + grup mesajı, mesaj tepkisi
    // =====================================================================================
    private async Task SeedChatAsync(User admin, User secondUser, CancellationToken ct)
    {
        var group = await GetOrAddAsync(_db.ChatGroups,
            g => g.Name == "Demo Proje Ekibi" && g.OwnerId == admin.Id,
            () => new ChatGroup { Id = Guid.NewGuid(), Name = "Demo Proje Ekibi", OwnerId = admin.Id }, ct);

        await GetOrAddAsync(_db.ChatGroupMembers,
            m => m.GroupId == group.Id && m.UserId == admin.Id,
            () => new ChatGroupMember
            {
                Id = Guid.NewGuid(), GroupId = group.Id, UserId = admin.Id, Status = ChatGroupMemberStatus.Accepted,
                IsOwner = true, IsAdmin = true,
            }, ct);

        if (secondUser.Id != admin.Id)
        {
            await GetOrAddAsync(_db.ChatGroupMembers,
                m => m.GroupId == group.Id && m.UserId == secondUser.Id,
                () => new ChatGroupMember
                {
                    Id = Guid.NewGuid(), GroupId = group.Id, UserId = secondUser.Id, Status = ChatGroupMemberStatus.Accepted,
                    IsOwner = false, IsAdmin = false, InvitedById = admin.Id,
                }, ct);
        }

        // Grup mesajı.
        await GetOrAddAsync(_db.ChatMessages,
            x => x.GroupId == group.Id && x.Text == "Gruba hoş geldiniz.",
            () => new ChatMessage
            {
                Id = Guid.NewGuid(), SenderId = admin.Id, GroupId = group.Id, Text = "Gruba hoş geldiniz.", IsRead = false,
            }, ct);

        // Birebir mesaj + tepki.
        if (secondUser.Id != admin.Id)
        {
            var directMessage = await GetOrAddAsync(_db.ChatMessages,
                x => x.SenderId == admin.Id && x.RecipientId == secondUser.Id && x.Text == "Merhaba, demo mesajı.",
                () => new ChatMessage
                {
                    Id = Guid.NewGuid(), SenderId = admin.Id, RecipientId = secondUser.Id, Text = "Merhaba, demo mesajı.", IsRead = false,
                }, ct);

            await GetOrAddAsync(_db.ChatMessageReactions,
                r => r.MessageId == directMessage.Id && r.UserId == secondUser.Id,
                () => new ChatMessageReaction
                {
                    Id = Guid.NewGuid(), MessageId = directMessage.Id, UserId = secondUser.Id, Emoji = "👍",
                }, ct);
        }
    }
}


