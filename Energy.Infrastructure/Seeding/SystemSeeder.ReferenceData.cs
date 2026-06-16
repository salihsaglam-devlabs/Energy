using BudgetEntity = Energy.Domain.Budget.Budget;
using Energy.Shared.Common;
using System.Text.RegularExpressions;
using Energy.Domain.Common;
using Energy.Domain.Core;
using Energy.Domain.IAM;
using Energy.Domain.Reporting;
using Energy.Domain.Workflow;
using Energy.Shared.Identity;
using Energy.Shared.Identity.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Energy.Infrastructure.Seeding;

/// <summary>
/// Kurumsal modüllerin (22 modül / 134 tablo) idempotent tohumlaması:
/// geçiş içermeyen şema sağlama, referans veriler (para birimi, ölçü birimi),
/// iş rolleri ve rol-yetki eşlemeleri, modül menüleri, dashboard widget'ları ve
/// varsayılan onay akışları. Tüm adımlar yeniden çalıştırılmaya güvenlidir.
/// </summary>
public sealed partial class SystemSeeder
{
    /// <summary>
    /// Para birimi tohum kayıtları (Seed Data sayfası). Görünen ad, sistemdeki diğer
    /// yerleşik kayıtlarla (DashboardWidgets, Roles, Menus, ApprovalDefinitions) tutarlı
    /// olacak şekilde gömülü metin yerine yerelleştirme anahtarı (<c>Currencies.{Code}.Name</c>)
    /// olarak saklanır ve gösterimde localizer ile çözülür.
    /// </summary>
    private static readonly (string Code, string Symbol)[] SeedCurrencies =
    [
        ("TRY", "₺"),
        ("USD", "$"),
        ("EUR", "€"),
    ];

    /// <summary>
    /// Ölçü birimi tohum kayıtları (Seed Data sayfası). Görünen ad, gömülü metin yerine
    /// yerelleştirme anahtarı (<c>Units.{Code}.Name</c>) olarak saklanır.
    /// </summary>
    private static readonly (string Code, string Symbol)[] SeedUnits =
    [
        ("Piece", "pcs"),
        ("Meter", "m"),
        ("Kilogram", "kg"),
        ("Ton", "t"),
        ("Liter", "L"),
        ("Hour", "h"),
        ("Day", "d"),
        ("Roll", "rl"),
        ("Package", "pkg"),
    ];

    /// <summary>
    /// Seed Data sayfasındaki iş rolleri ve her birinin yetki kümesi. Yetkiler
    /// merkezi <see cref="PermissionCatalog"/>'tan modül CRUD genişletmesiyle üretilir.
    /// </summary>
    private static readonly (string Name, string[] Modules, string[] Extra)[] BusinessRoles =
    [
        ("ProjectManager",
            ["Projects", "Requests", "Operations", "FieldOperations", "Documents", "Reporting"],
            ["Inventory.Read", "Procurement.Read", "Workflow.Approve", "Workflow.Reject", "Workflow.Return", "Reporting.Export"]),
        ("WarehouseManager",
            ["Inventory", "Catalog"],
            ["Inventory.Approve", "Inventory.Transfer", "Inventory.Count", "Inventory.Reverse", "Requests.Read"]),
        ("PurchaseManager",
            ["Procurement", "Requests"],
            ["Procurement.Approve", "Inventory.Read", "Catalog.Read", "Workflow.Approve", "Workflow.Reject"]),
        ("FinanceManager",
            ["Finance", "BudgetEntity", "Contracts", "ProgressPayments"],
            ["Workflow.Approve", "Workflow.Reject", "Reporting.Export", "Reporting.Read"]),
        ("HRManager",
            ["HR", "Organization"],
            ["Workflow.Approve", "Workflow.Reject", "Reporting.Read"]),
        ("SiteSupervisor",
            ["FieldOperations", "Operations"],
            ["Projects.Read", "Inventory.Read", "Documents.Upload", "Documents.Download"]),
    ];

    /// <summary>Kurumsal veri tohumlamasını sırayla çalıştırır (şema ayrı, daha erken sağlanır).</summary>
    private async Task SeedReferenceAndOperationalDataAsync(CancellationToken ct)
    {

        _logger.LogInformation("Seeding: enterprise reference data (currencies, units)");
        await EnsureModuleReferenceDataAsync(ct);

        _logger.LogInformation("Seeding: business roles + permission grants");
        await EnsureBusinessRolesAsync(ct);

        _logger.LogInformation("Seeding: enterprise module menus");
        await EnsureModuleMenusAsync(ct);

        _logger.LogInformation("Seeding: per-entity module menus");
        await EnsureEntityMenusAsync(ct);

        _logger.LogInformation("Seeding: per-report module menus");
        await EnsureReportMenusAsync(ct);

        _logger.LogInformation("Seeding: per-process module menus");
        await EnsureProcessMenusAsync(ct);

        _logger.LogInformation("Seeding: dashboard widgets");
        await EnsureDashboardWidgetsAsync(ct);

        _logger.LogInformation("Seeding: default approval definitions");
        await EnsureDefaultApprovalDefinitionsAsync(ct);

        _logger.LogInformation("Seeding: sample business data (header + line demo records)");
        await EnsureSampleBusinessDataAsync(ct);

        _logger.LogInformation("Seeding: full sample data across every remaining table");
        await EnsureFullSampleDataAsync(ct);
    }

    /// <summary>
    /// Kurumsal tabloları idempotent şekilde sağlar. SQL Server'da taze bir veritabanı
    /// EnsureCreated ile zaten tüm tabloları oluşturur; bu adım yalnızca tablolar henüz
    /// yoksa (örn. yalnızca IAM tablolarını içeren mevcut bir veritabanı) modelden
    /// üretilen oluşturma betiğini ifade ifade çalıştırır ve var olan nesneleri atlar.
    /// </summary>
    private async Task EnsureModuleTablesSchemaAsync(CancellationToken ct)
    {
        if (await TableExistsAsync("Companies", ct))
        {
            return;
        }

        string script = _db.Database.GenerateCreateScript();
        var statements = SplitSqlStatements(script);

        var created = 0;
        foreach (var statement in statements)
        {
            try
            {
                await _db.Database.ExecuteSqlRawAsync(statement, ct);
                created++;
            }
            catch (Exception ex)
            {
                // Var olan nesneler (IAM/Chat tabloları) yeniden oluşturulamaz; bu beklenen bir durumdur.
                _logger.LogDebug(ex, "Enterprise schema statement skipped (object may already exist).");
            }
        }

        _logger.LogInformation("Enterprise schema: {Count} statement(s) applied.", created);
    }

    private async Task<bool> TableExistsAsync(string tableName, CancellationToken ct)
    {
        try
        {
            var count = await _db.Database
                .SqlQueryRaw<int>(
                    "SELECT COUNT(*) AS \"Value\" FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = {0}",
                    tableName)
                .FirstOrDefaultAsync(ct);
            return count > 0;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not determine whether table {Table} exists; assuming it does.", tableName);
            return true;
        }
    }

    private IEnumerable<string> SplitSqlStatements(string script)
    {
        if (_db.Database.IsSqlServer())
        {
            return Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline)
                .Select(s => s.Trim())
                .Where(s => s.Length > 0);
        }

        return script.Split(';')
            .Select(s => s.Trim())
            .Where(s => s.Length > 0);
    }

    private async Task EnsureModuleReferenceDataAsync(CancellationToken ct)
    {
        foreach (var (code, symbol) in SeedCurrencies)
        {
            if (!await _db.Currencies.IgnoreQueryFilters().AnyAsync(c => c.Code == code, ct))
            {
                _db.Currencies.Add(new Currency { Id = Guid.NewGuid(), Code = code, Name = $"Currencies.{code}.Name", Symbol = symbol, IsActive = true });
            }
        }

        foreach (var (code, symbol) in SeedUnits)
        {
            if (!await _db.UnitsOfMeasure.IgnoreQueryFilters().AnyAsync(u => u.Code == code, ct))
            {
                _db.UnitsOfMeasure.Add(new UnitOfMeasure { Id = Guid.NewGuid(), Code = code, Name = $"Units.{code}.Name", Symbol = symbol, IsActive = true });
            }
        }

        await _db.SaveChangesAsync(ct);
    }

    private async Task EnsureBusinessRolesAsync(CancellationToken ct)
    {
        // Admin = tüm yetkiler (SuperAdmin'den farklı; yetki tabanlı, yönetilebilir).
        await EnsureRoleWithPermissionsAsync("Admin", [.. PermissionCatalog.All.Select(p => p.Code)], ct);

        foreach (var (name, modules, extra) in BusinessRoles)
        {
            var codes = new List<string>();
            foreach (var module in modules)
            {
                codes.Add($"{module}.{PermissionActions.Read}");
                codes.Add($"{module}.{PermissionActions.ReadAll}");
                codes.Add($"{module}.{PermissionActions.Create}");
                codes.Add($"{module}.{PermissionActions.Update}");
                codes.Add($"{module}.{PermissionActions.Delete}");
            }
            codes.AddRange(extra);
            codes.Add(PermissionCatalog.DashboardRead);

            await EnsureRoleWithPermissionsAsync(name, codes, ct);
        }

        // Her iş rolü için giriş yapılabilir bir demo kullanıcısı sağla; böylece
        // roller yalnızca tanımlı değil, uçtan uca denenebilir olur.
        await EnsureBusinessRoleDemoUsersAsync(ct);
    }

    /// <summary>İş rolü adı → demo kullanıcı bilgileri (kullanıcı adı, e-posta, ad, soyad, parola).</summary>
    private static readonly (string Role, string UserName, string Email, string FirstName, string LastName, string Password)[] BusinessRoleDemoUsers =
    [
        ("ProjectManager",   "project.manager",   "project.manager@energy.local",   "Burak", "Aslan",   "Project123!"),
        ("WarehouseManager", "warehouse.manager", "warehouse.manager@energy.local", "Cem",   "Korkmaz", "Warehouse123!"),
        ("PurchaseManager",  "purchase.manager",  "purchase.manager@energy.local",  "Gizem", "Arslan",  "Purchase123!"),
        ("FinanceManager",   "finance.manager",   "finance.manager@energy.local",   "Derya", "Polat",   "Finance123!"),
        ("HRManager",        "hr.manager",        "hr.manager@energy.local",        "Ece",   "Doğan",   "HumanRes123!"),
        ("SiteSupervisor",   "site.supervisor",   "site.supervisor@energy.local",   "Onur",  "Şimşek",  "SiteSup123!"),
    ];

    /// <summary>
    /// İş rollerine birer demo kullanıcı bağlar. Kullanıcı e-postaya göre idempotent
    /// sağlanır ve rol ataması yalnızca yoksa eklenir; mevcut kayıtlar değiştirilmez.
    /// </summary>
    private async Task EnsureBusinessRoleDemoUsersAsync(CancellationToken ct)
    {
        var usersAdded = 0;
        foreach (var (roleName, userName, email, firstName, lastName, password) in BusinessRoleDemoUsers)
        {
            var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == roleName, ct);
            if (role is null)
            {
                continue;
            }

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
            if (user is null)
            {
                user = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = userName,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    PasswordHash = _passwords.Hash(password),
                    IsActive = true,
                    SecurityStamp = Guid.NewGuid(),
                };
                _db.Users.Add(user);
                await _db.SaveChangesAsync(ct);
                usersAdded++;
            }

            if (!await _db.UserRoles.AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id, ct))
            {
                _db.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id });
                await _db.SaveChangesAsync(ct);
                _permissionResolver.InvalidateUser(user.Id);
            }
        }

        if (usersAdded > 0)
        {
            _logger.LogInformation("Business roles: {Count} demo user(s) ensured.", usersAdded);
        }
    }

    private async Task EnsureRoleWithPermissionsAsync(string roleName, IReadOnlyCollection<string> permissionCodes, CancellationToken ct)
    {
        var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == roleName, ct);
        if (role is null)
        {
            role = new Role { Id = Guid.NewGuid(), Name = roleName, Description = $"Roles.{roleName}.Description", IsSystem = false };
            _db.Roles.Add(role);
            await _db.SaveChangesAsync(ct);
        }

        var existing = (await _db.RolePermissions
                .Where(rp => rp.RoleId == role.Id)
                .Select(rp => rp.PermissionCode)
                .ToListAsync(ct))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var validCodes = PermissionCatalog.AllCodes;
        var changed = false;
        foreach (var code in permissionCodes.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            if (existing.Contains(code) || !validCodes.Contains(code))
            {
                continue;
            }
            _db.RolePermissions.Add(new RolePermission { RoleId = role.Id, PermissionCode = code });
            changed = true;
        }

        if (changed)
        {
            await _db.SaveChangesAsync(ct);
            await _permissionResolver.InvalidateRoleAsync(role.Id, ct);
        }
    }

    private async Task EnsureModuleMenusAsync(CancellationToken ct)
    {
        // Alan bazlı üst menüler + modül girdileri. Her modül kendi <Module>.ReadAll yetkisiyle korunur.
        var projects = await EnsureMenuAsync("Menus.ProjectsArea", null, null, "hierarchy", 20, null, ct);
        await EnsureMenuAsync("Menus.Projects", projects.Id, null, "box", 21, "Projects.ReadAll", ct);
        await EnsureMenuAsync("Menus.Operations", projects.Id, null, "preferences", 22, "Operations.ReadAll", ct);
        await EnsureMenuAsync("Menus.FieldOperations", projects.Id, null, "map", 23, "FieldOperations.ReadAll", ct);
        await EnsureMenuAsync("Menus.Contracts", projects.Id, null, "doc", 24, "Contracts.ReadAll", ct);
        await EnsureMenuAsync("Menus.ProgressPayments", projects.Id, null, "money", 25, "ProgressPayments.ReadAll", ct);

        var supply = await EnsureMenuAsync("Menus.SupplyArea", null, null, "cart", 30, null, ct);
        await EnsureMenuAsync("Menus.Catalog", supply.Id, null, "detailslayout", 31, "Catalog.ReadAll", ct);
        await EnsureMenuAsync("Menus.Inventory", supply.Id, null, "box", 32, "Inventory.ReadAll", ct);
        await EnsureMenuAsync("Menus.Requests", supply.Id, null, "newfolder", 33, "Requests.ReadAll", ct);
        await EnsureMenuAsync("Menus.Procurement", supply.Id, null, "cart", 34, "Procurement.ReadAll", ct);

        var finance = await EnsureMenuAsync("Menus.FinanceArea", null, null, "money", 40, null, ct);
        await EnsureMenuAsync("Menus.Finance", finance.Id, null, "money", 41, "Finance.ReadAll", ct);
        await EnsureMenuAsync("Menus.Budget", finance.Id, null, "chart", 42, "BudgetEntity.ReadAll", ct);

        var hr = await EnsureMenuAsync("Menus.HRArea", null, null, "group", 50, null, ct);
        await EnsureMenuAsync("Menus.Organization", hr.Id, null, "group", 51, "Organization.ReadAll", ct);
        await EnsureMenuAsync("Menus.HR", hr.Id, null, "clock", 52, "HR.ReadAll", ct);
        await EnsureMenuAsync("Menus.Assets", hr.Id, null, "car", 53, "Assets.ReadAll", ct);

        var common = await EnsureMenuAsync("Menus.CommonArea", null, null, "more", 60, null, ct);
        await EnsureMenuAsync("Menus.BusinessPartners", common.Id, null, "card", 61, "BusinessPartners.ReadAll", ct);
        await EnsureMenuAsync("Menus.Documents", common.Id, null, "doc", 62, "Documents.ReadAll", ct);
        await EnsureMenuAsync("Menus.Workflow", common.Id, null, "check", 63, "Workflow.ReadAll", ct);
        await EnsureMenuAsync("Menus.Notifications", common.Id, null, "bell", 64, "Notifications.ReadAll", ct);
        await EnsureMenuAsync("Menus.Reporting", common.Id, null, "chart", 65, "Reporting.ReadAll", ct);
        await EnsureMenuAsync("Menus.CoreData", common.Id, null, "preferences", 66, "Core.ReadAll", ct);
    }

    private async Task EnsureDashboardWidgetsAsync(CancellationToken ct)
    {
        (string Code, string Module, string Type, int Order, string? Perm)[] widgets =
        [
            ("LowStock", "Inventory", "Counter", 1, "Inventory.ReadAll"),
            ("PendingApprovals", "Workflow", "Counter", 2, "Workflow.ReadAll"),
            ("BudgetOverrun", "BudgetEntity", "Chart", 3, "BudgetEntity.ReadAll"),
            ("OrderDelivery", "Procurement", "Grid", 4, "Procurement.ReadAll"),
            ("WorkOrderProgress", "Operations", "Gauge", 5, "Operations.ReadAll"),
        ];

        foreach (var (code, module, type, order, perm) in widgets)
        {
            if (!await _db.DashboardWidgets.IgnoreQueryFilters().AnyAsync(w => w.Code == code, ct))
            {
                _db.DashboardWidgets.Add(new DashboardWidget
                {
                    Id = Guid.NewGuid(),
                    Code = code,
                    Name = $"DashboardWidgets.{code}.Name",
                    Module = module,
                    WidgetType = type,
                    RequiredPermissionCode = perm,
                    DisplayOrder = order,
                    IsActive = true,
                });
            }
        }

        await _db.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Tasarım dokümanındaki örnek satın alma ve masraf onay akışlarını idempotent
    /// olarak oluşturur (definition + yürürlükteki versiyon + adımlar + onaycılar).
    /// </summary>
    private async Task EnsureDefaultApprovalDefinitionsAsync(CancellationToken ct)
    {
        await EnsureApprovalDefinitionAsync(
            code: "PurchaseOrderApproval",
            relatedModule: "Procurement",
            relatedEntityType: "PurchaseOrder",
            steps:
            [
                (1, "ProjectManager", ApprovalMode.Sequential),
                (2, "FinanceManager", ApprovalMode.Sequential),
            ],
            ct);

        await EnsureApprovalDefinitionAsync(
            code: "ExpenseClaimApproval",
            relatedModule: "Organization",
            relatedEntityType: "ExpenseClaim",
            steps:
            [
                (1, "ProjectManager", ApprovalMode.ParallelAny),
                (2, "FinanceManager", ApprovalMode.Sequential),
            ],
            ct);
    }

    private async Task EnsureApprovalDefinitionAsync(
        string code,
        string relatedModule,
        string relatedEntityType,
        IReadOnlyList<(int StepNo, string RoleName, ApprovalMode Mode)> steps,
        CancellationToken ct)
    {
        if (await _db.ApprovalDefinitions.IgnoreQueryFilters().AnyAsync(d => d.Code == code, ct))
        {
            return;
        }

        var definition = new ApprovalDefinition
        {
            Id = Guid.NewGuid(),
            Code = code,
            Name = $"ApprovalDefinitions.{code}.Name",
            RelatedModule = relatedModule,
            RelatedEntityType = relatedEntityType,
            IsActive = true,
        };
        _db.ApprovalDefinitions.Add(definition);

        var version = new ApprovalDefinitionVersion
        {
            Id = Guid.NewGuid(),
            ApprovalDefinitionId = definition.Id,
            VersionNo = 1,
            EffectiveFrom = DateTime.UtcNow,
            IsActive = true,
        };
        _db.ApprovalDefinitionVersions.Add(version);

        foreach (var (stepNo, roleName, mode) in steps)
        {
            var step = new ApprovalStepDefinition
            {
                Id = Guid.NewGuid(),
                ApprovalDefinitionVersionId = version.Id,
                StepNo = stepNo,
                Name = $"ApprovalSteps.{code}.{stepNo}",
                ApprovalMode = mode,
                IsRequired = true,
            };
            _db.ApprovalStepDefinitions.Add(step);

            var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == roleName, ct);
            _db.ApprovalStepApprovers.Add(new ApprovalStepApprover
            {
                Id = Guid.NewGuid(),
                ApprovalStepDefinitionId = step.Id,
                ApproverType = ApproverType.Role,
                ApproverRoleId = role?.Id,
            });
        }

        await _db.SaveChangesAsync(ct);
    }
}

