using Energy.Infrastructure.Persistence;
using Energy.Infrastructure.Modules.Catalog.MaterialCategoryAttribute.Services;
using Energy.Shared.Models.V1.Catalog.MaterialCategoryAttribute.Requests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Energy.Tests.Modules.Catalog;

/// <summary>MaterialCategoryAttribute CRUD servisi round-trip testi (EF InMemory).</summary>
public sealed class MaterialCategoryAttributeServiceTests
{
    private static EnergyDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<EnergyDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new EnergyDbContext(options);
    }

    [Fact]
    public async Task Create_Get_Update_Delete_RoundTrips()
    {
        await using var db = NewContext();
        var service = new MaterialCategoryAttributeService(db);

        var created = await service.CreateAsync(new CreateMaterialCategoryAttributeRequest());
        Assert.True(created.IsSuccess);
        var id = created.Data;

        var detail = await service.GetByIdAsync(id);
        Assert.True(detail.IsSuccess);

        var updated = await service.UpdateAsync(id, new UpdateMaterialCategoryAttributeRequest { Id = id });
        Assert.True(updated.IsSuccess);

        var deleted = await service.DeleteAsync(id);
        Assert.True(deleted.IsSuccess);
    }

    [Fact]
    public async Task GetById_Unknown_ReturnsFailure()
    {
        await using var db = NewContext();
        var service = new MaterialCategoryAttributeService(db);
        var result = await service.GetByIdAsync(Guid.NewGuid());
        Assert.False(result.IsSuccess);
    }
}
