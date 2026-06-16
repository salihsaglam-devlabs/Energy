#!/usr/bin/env python3
"""
Energy — Infrastructure layer generator (Phase 4b/5).

Emits:
  * EnergyDbContext  — canonical context mapping all 134 Modules entities,
    applying the per-entity Modules configurations + audit-user FK convention
    + global soft-delete filter.
  * Per-entity {Entity}Service  — CRUD with AsNoTracking, projection, pagination,
    soft-delete (implements Application I{Entity}Service).
  * Per-entity {Entity}LookupService — active/search filtered lookup projection.
  * ModulesServiceRegistration — DI extension registering all of the above.

Additive (new namespaces). The canonical context is wired into DI at the final
cutover; until then the registration extension is self-contained and compiles.
"""
from __future__ import annotations

import os
import shutil

from generate_domain import ROOT, AUDIT_COLUMNS, build_model

PERSIST = os.path.join(ROOT, "Energy.Infrastructure", "Persistence")
INFRA_MODULES = os.path.join(ROOT, "Energy.Infrastructure", "Modules")

FULL_AUDIT = {"CreatedAt", "CreatedBy", "UpdatedAt", "UpdatedBy",
              "IsDeleted", "DeletedAt", "DeletedBy"}


def write(path, content):
    os.makedirs(os.path.dirname(path), exist_ok=True)
    with open(path, "w", encoding="utf-8") as f:
        f.write(content)


def colnames(cols):
    return {c["col"] for c in cols}


def input_columns(cols):
    return [c for c in cols if c["col"] not in AUDIT_COLUMNS]


def gen_context(order, table_module, table_entity):
    modules = sorted({table_module[t] for t in order})
    lines = ["using Microsoft.EntityFrameworkCore;",
             "using System.Reflection;"]
    for m in modules:
        lines.append(f"using Energy.Domain.Modules.{m};")
    lines += [
        "using Energy.Domain.Common;",
        "using Energy.Domain.Modules.IAM;", "",
        "namespace Energy.Infrastructure.Persistence;", "",
        "/// <summary>",
        "/// Kanonik (Modules) EF Core bağlamı. 134 tablonun tamamını per-entity",
        "/// yapılandırmalar + audit FK + soft-delete konvansiyonlarıyla eşleştirir.",
        "/// </summary>",
        "public class EnergyDbContext : DbContext",
        "{",
        "    public EnergyDbContext(DbContextOptions<EnergyDbContext> options) : base(options)",
        "    {",
        "    }", "",
    ]
    for t in order:
        e = table_entity[t]
        lines.append(f"    public DbSet<global::Energy.Domain.Modules.{table_module[t]}.{e}> {t} => Set<global::Energy.Domain.Modules.{table_module[t]}.{e}>();")
    lines += [
        "",
        "    protected override void ConfigureConventions(ModelConfigurationBuilder b)",
        "    {",
        "        b.Properties<decimal>().HavePrecision(18, 6);",
        "    }", "",
        "    protected override void OnModelCreating(ModelBuilder builder)",
        "    {",
        "        builder.ApplyConfigurationsFromAssembly(",
        "            typeof(EnergyDbContext).Assembly,",
        "            type => type.Namespace?.StartsWith(",
        '                "Energy.Infrastructure.Persistence.Configurations.Modules",',
        "                StringComparison.Ordinal) ?? false);", "",
        "        ApplyAuditUserForeignKeys(builder);",
        "        ApplySoftDeleteConvention(builder);",
        "    }", "",
        "    private static void ApplyAuditUserForeignKeys(ModelBuilder builder)",
        "    {",
        "        foreach (var entityType in builder.Model.GetEntityTypes())",
        "        {",
        "            if (!typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType)) continue;",
        "            typeof(EnergyDbContext)",
        '                .GetMethod(nameof(ApplyAuditUserFk), BindingFlags.NonPublic | BindingFlags.Static)!',
        "                .MakeGenericMethod(entityType.ClrType).Invoke(null, [builder]);",
        "        }",
        "    }", "",
        "    private static void ApplyAuditUserFk<TEntity>(ModelBuilder builder) where TEntity : AuditableEntity",
        "    {",
        "        var entity = builder.Entity<TEntity>();",
        "        entity.HasOne<User>().WithMany().HasForeignKey(e => e.CreatedBy).OnDelete(DeleteBehavior.Restrict);",
        "        entity.HasOne<User>().WithMany().HasForeignKey(e => e.UpdatedBy).OnDelete(DeleteBehavior.Restrict);",
        "        entity.HasOne<User>().WithMany().HasForeignKey(e => e.DeletedBy).OnDelete(DeleteBehavior.Restrict);",
        "    }", "",
        "    private static void ApplySoftDeleteConvention(ModelBuilder builder)",
        "    {",
        "        foreach (var entityType in builder.Model.GetEntityTypes())",
        "        {",
        "            if (!typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType)) continue;",
        "            if (entityType.GetDeclaredQueryFilters().Any()) continue;",
        "            typeof(EnergyDbContext)",
        '                .GetMethod(nameof(ApplySoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static)!',
        "                .MakeGenericMethod(entityType.ClrType).Invoke(null, [builder]);",
        "        }",
        "    }", "",
        "    private static void ApplySoftDeleteFilter<TEntity>(ModelBuilder builder) where TEntity : AuditableEntity",
        "        => builder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted);",
        "}", "",
    ]
    return "\n".join(lines)


def gen_service(module, entity, table, cols):
    names = colnames(cols)
    has_created = "CreatedAt" in names
    has_audit = FULL_AUDIT.issubset(names)
    etype = f"global::Energy.Domain.Modules.{module}.{entity}"
    ns = f"Energy.Infrastructure.Modules.{module}.{entity}.Services"
    ins = input_columns(cols)

    list_map = ["                Id = e.Id"]
    for c in ins:
        list_map.append(f"                {c['col']} = e.{c['col']}")
    list_map.append("                CreatedAt = " + ("e.CreatedAt" if has_created else "default"))
    detail_map = ["                Id = e.Id"]
    for c in cols:
        if c["col"] == "Id":
            continue
        detail_map.append(f"                {c['col']} = e.{c['col']}")
    create_set = [f"            {c['col']} = request.{c['col']}," for c in ins]
    update_set = [f"            entity.{c['col']} = request.{c['col']};" for c in ins]

    L = [
        "using Microsoft.EntityFrameworkCore;",
        "using Energy.Infrastructure.Persistence;",
        "using Energy.Shared.Models.V1.Common.Responses;",
        f"using Energy.Application.Modules.{module}.{entity}.Services;",
        f"using Energy.Shared.Models.V1.{module}.{entity}.Requests;",
        f"using Energy.Shared.Models.V1.{module}.{entity}.Responses;", "",
        f"namespace {ns};", "",
        f"/// <summary>{entity} CRUD servisi (projection, pagination, soft-delete).</summary>",
        f"public class {entity}Service : I{entity}Service",
        "{",
        "    private readonly EnergyDbContext _db;", "",
        f"    public {entity}Service(EnergyDbContext db) => _db = db;", "",
        f"    public async Task<BaseResponse<PaginatedResponse<{entity}ListResponse>>> GetListAsync(Get{entity}ListRequest request, CancellationToken ct = default)",
        "    {",
        f"        var query = _db.{table}.AsNoTracking();",
        "        var total = await query.CountAsync(ct);",
        "        var items = await query",
        "            .OrderByDescending(e => e.Id)",
        "            .Skip((request.PageNumber - 1) * request.PageSize)",
        "            .Take(request.PageSize)",
        f"            .Select(e => new {entity}ListResponse",
        "            {",
        ",\n".join(list_map),
        "            })",
        "            .ToListAsync(ct);",
        f"        var page = PaginatedResponse<{entity}ListResponse>.Create(items, request.PageNumber, request.PageSize, total);",
        f"        return BaseResponse<PaginatedResponse<{entity}ListResponse>>.Success(page);",
        "    }", "",
        f"    public async Task<BaseResponse<{entity}DetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)",
        "    {",
        f"        var dto = await _db.{table}.AsNoTracking().Where(e => e.Id == id)",
        f"            .Select(e => new {entity}DetailResponse",
        "            {",
        ",\n".join(detail_map),
        "            }).FirstOrDefaultAsync(ct);",
        f"        return dto is null",
        f'            ? BaseResponse<{entity}DetailResponse>.Failure("NotFound")',
        f"            : BaseResponse<{entity}DetailResponse>.Success(dto);",
        "    }", "",
        f"    public async Task<BaseResponse<Guid>> CreateAsync(Create{entity}Request request, CancellationToken ct = default)",
        "    {",
        f"        var entity = new {etype}",
        "        {",
        "            Id = Guid.NewGuid(),",
    ]
    L += create_set
    if has_created:
        L.append("            CreatedAt = DateTime.UtcNow,")
    L += [
        "        };",
        f"        _db.{table}.Add(entity);",
        "        await _db.SaveChangesAsync(ct);",
        '        return BaseResponse<Guid>.Success(entity.Id, "Created");',
        "    }", "",
        f"    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, Update{entity}Request request, CancellationToken ct = default)",
        "    {",
        f"        var entity = await _db.{table}.FirstOrDefaultAsync(e => e.Id == id, ct);",
        '        if (entity is null) return BaseResponse<bool>.Failure("NotFound");',
    ]
    L += update_set
    if "UpdatedAt" in names:
        L.append("        entity.UpdatedAt = DateTime.UtcNow;")
    L += [
        "        await _db.SaveChangesAsync(ct);",
        '        return BaseResponse<bool>.Success(true, "Updated");',
        "    }", "",
        f"    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)",
        "    {",
        f"        var entity = await _db.{table}.FirstOrDefaultAsync(e => e.Id == id, ct);",
        '        if (entity is null) return BaseResponse<bool>.Failure("NotFound");',
    ]
    if has_audit:
        L += [
            "        entity.IsDeleted = true;",
            "        entity.DeletedAt = DateTime.UtcNow;",
        ]
    else:
        L.append(f"        _db.{table}.Remove(entity);")
    L += [
        "        await _db.SaveChangesAsync(ct);",
        '        return BaseResponse<bool>.Success(true, "Deleted");',
        "    }",
        "}", "",
    ]
    return "\n".join(L)


def gen_lookup(module, entity, table, cols):
    names = colnames(cols)
    has_code = "Code" in names
    has_name = "Name" in names
    has_active = "IsActive" in names
    ns = f"Energy.Infrastructure.Modules.{module}.{entity}.Lookups"
    code_expr = "e.Code" if has_code else "null"
    name_expr = "e.Name" if has_name else "null"
    if has_name:
        disp = "e.Name"
    elif has_code:
        disp = "e.Code"
    else:
        disp = "e.Id.ToString()"
    active_expr = "e.IsActive" if has_active else "true"

    L = [
        "using Microsoft.EntityFrameworkCore;",
        "using Energy.Infrastructure.Persistence;",
        "using Energy.Shared.Models.V1.Common.Responses;",
        f"using Energy.Application.Modules.{module}.{entity}.Lookups;",
        f"using Energy.Shared.Models.V1.{module}.{entity}.Responses;", "",
        f"namespace {ns};", "",
        f"/// <summary>{entity} lookup servisi (aktif + arama filtreli projection).</summary>",
        f"public class {entity}LookupService : I{entity}LookupService",
        "{",
        "    private readonly EnergyDbContext _db;", "",
        f"    public {entity}LookupService(EnergyDbContext db) => _db = db;", "",
        f"    public async Task<BaseResponse<IReadOnlyList<{entity}LookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)",
        "    {",
        f"        var query = _db.{table}.AsNoTracking();",
    ]
    if has_active:
        L.append("        if (activeOnly) query = query.Where(e => e.IsActive);")
    if has_name:
        L.append("        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search));")
    elif has_code:
        L.append("        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Code.Contains(search));")
    L += [
        f"        var items = await query.Select(e => new {entity}LookupResponse",
        "        {",
        "            Id = e.Id,",
        f"            Code = {code_expr},",
        f"            Name = {name_expr},",
        f"            DisplayName = {disp},",
        f"            IsActive = {active_expr}",
        "        }).ToListAsync(ct);",
        f"        return BaseResponse<IReadOnlyList<{entity}LookupResponse>>.Success(items);",
        "    }",
        "}", "",
    ]
    return "\n".join(L)


def gen_registration(order, table_module, table_entity):
    L = [
        "using Microsoft.Extensions.DependencyInjection;", "",
        "namespace Energy.Infrastructure.Modules;", "",
        "/// <summary>Tüm per-entity Modules servis ve lookup kayıtları (DI).</summary>",
        "public static class ModulesServiceRegistration",
        "{",
        "    public static IServiceCollection AddModulesEntityServices(this IServiceCollection services)",
        "    {",
    ]
    for t in order:
        m, e = table_module[t], table_entity[t]
        L.append(f"        services.AddScoped<global::Energy.Application.Modules.{m}.{e}.Services.I{e}Service, global::Energy.Infrastructure.Modules.{m}.{e}.Services.{e}Service>();")
        L.append(f"        services.AddScoped<global::Energy.Application.Modules.{m}.{e}.Lookups.I{e}LookupService, global::Energy.Infrastructure.Modules.{m}.{e}.Lookups.{e}LookupService>();")
    L += ["        return services;", "    }", "}", ""]
    return "\n".join(L)


def main():
    order, table_module, table_purpose, table_columns, table_entity = build_model()

    # Non-destructive cleanup: remove ONLY the per-entity generated folders, so
    # hand-written / separately-generated infra (Reports, Processes, Files) survive.
    for t in order:
        m, e = table_module[t], table_entity[t]
        shutil.rmtree(os.path.join(INFRA_MODULES, m, e), ignore_errors=True)

    write(os.path.join(PERSIST, "EnergyDbContext.cs"),
          gen_context(order, table_module, table_entity))

    svc = look = 0
    for t in order:
        m, e = table_module[t], table_entity[t]
        cols = table_columns.get(t, [])
        write(os.path.join(INFRA_MODULES, m, e, "Services", f"{e}Service.cs"),
              gen_service(m, e, t, cols))
        write(os.path.join(INFRA_MODULES, m, e, "Lookups", f"{e}LookupService.cs"),
              gen_lookup(m, e, t, cols))
        svc += 1
        look += 1
    write(os.path.join(INFRA_MODULES, "ModulesServiceRegistration.cs"),
          gen_registration(order, table_module, table_entity))
    print(f"EnergyDbContext + {svc} services + {look} lookup services + registration generated")


if __name__ == "__main__":
    main()

