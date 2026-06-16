using Energy.Infrastructure.Persistence;
using Energy.Infrastructure.Modules.IAM.Permission.Services;
using Energy.Shared.Models.V1.IAM.Permission.Requests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Energy.Tests.Modules.IAM;

/// <summary>Permission CRUD servisi round-trip testi (EF InMemory).</summary>
public sealed class PermissionServiceTests
{
    private static AppDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task List_Works_And_Surrogate_Mutations_NotSupported()
    {
        // Doğal/bileşik anahtarlı veya append-only kayıtlar surrogate Guid ile
        // yönetilmez; salt-okunur liste desteklenir, mutasyonlar NotSupported döner.
        await using var db = NewContext();
        var service = new PermissionService(db);

        var list = await service.GetListAsync(new GetPermissionListRequest { PageNumber = 1, PageSize = 10 });
        Assert.True(list.IsSuccess);

        Assert.False((await service.GetByIdAsync(Guid.NewGuid())).IsSuccess);
        Assert.False((await service.UpdateAsync(Guid.NewGuid(), new UpdatePermissionRequest())).IsSuccess);
        Assert.False((await service.DeleteAsync(Guid.NewGuid())).IsSuccess);
    }

    [Fact]
    public async Task GetById_Unknown_ReturnsFailure()
    {
        await using var db = NewContext();
        var service = new PermissionService(db);
        var result = await service.GetByIdAsync(Guid.NewGuid());
        Assert.False(result.IsSuccess);
    }
}
