using Energy.Infrastructure.System.Services;
using Energy.Shared.Identity.Permissions;
using Xunit;

namespace Energy.Tests;

/// <summary>
/// Route/permission katalog kapsama testi (spec §22). Üretilen API uç noktalarının
/// (entity + rapor) eşlendiği TÜM yetki kodlarının merkezi <see cref="PermissionCatalog"/>
/// içinde tanımlı olduğunu doğrular; böylece seeder'ın oluşturmadığı bir yetkiye
/// eşlenen ("yetim") bir uç nokta derleme/CI aşamasında yakalanır.
/// </summary>
public sealed class PermissionCoverageTests
{
    private static IReadOnlyCollection<string> CollectMappedPermissions()
    {
        var map = new Dictionary<string, string?>(StringComparer.Ordinal);
        EntityEndpointPermissionMap.Apply(map);
        ReportEndpointPermissionMap.Apply(map);
        return map.Values
            .Where(v => !string.IsNullOrWhiteSpace(v))
            .Select(v => v!)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    [Fact]
    public void Every_Mapped_Endpoint_Permission_Exists_In_Catalog()
    {
        var mapped = CollectMappedPermissions();
        var missing = mapped.Where(code => !PermissionCatalog.AllCodes.Contains(code)).ToList();

        Assert.True(missing.Count == 0,
            "Endpoint permission map references codes absent from PermissionCatalog: " + string.Join(", ", missing));
    }

    [Fact]
    public void Mapped_Permissions_Are_Not_Empty()
    {
        Assert.NotEmpty(CollectMappedPermissions());
    }

    [Fact]
    public void Catalog_Has_No_Malformed_Codes()
    {
        // Her kod 'Module.Action' (en az 2 parça) biçiminde olmalı.
        var malformed = PermissionCatalog.AllCodes
            .Where(code => code.Split('.').Length < 2 || code.StartsWith('.') || code.EndsWith('.'))
            .ToList();
        Assert.True(malformed.Count == 0, "Malformed permission codes: " + string.Join(", ", malformed));
    }
}

