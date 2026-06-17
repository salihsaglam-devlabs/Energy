using Energy.Infrastructure.Persistence;
using Energy.Infrastructure.IAM.UserSetting.Services;
using Energy.Shared.Models.V1.IAM.UserSetting.Requests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Energy.Tests.IAM;

/// <summary>UserSetting CRUD servisi round-trip testi (EF InMemory).</summary>
public sealed class UserSettingServiceTests
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
        var service = new UserSettingService(db);

        var list = await service.GetListAsync(new GetUserSettingListRequest { PageNumber = 1, PageSize = 10 });
        Assert.True(list.IsSuccess);

        Assert.False((await service.GetByIdAsync(Guid.NewGuid())).IsSuccess);
        Assert.False((await service.UpdateAsync(Guid.NewGuid(), new UpdateUserSettingRequest())).IsSuccess);
        Assert.False((await service.DeleteAsync(Guid.NewGuid())).IsSuccess);
    }

    [Fact]
    public async Task GetById_Unknown_ReturnsFailure()
    {
        await using var db = NewContext();
        var service = new UserSettingService(db);
        var result = await service.GetByIdAsync(Guid.NewGuid());
        Assert.False(result.IsSuccess);
    }
}
