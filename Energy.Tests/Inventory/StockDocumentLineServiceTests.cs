using Energy.Infrastructure.Persistence;
using Energy.Infrastructure.Inventory.StockDocumentLine.Services;
using Energy.Shared.Models.V1.Inventory.StockDocumentLine.Requests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Energy.Tests.Inventory;

/// <summary>StockDocumentLine CRUD servisi round-trip testi (EF InMemory).</summary>
public sealed class StockDocumentLineServiceTests
{
    private static AppDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task Create_Get_Update_Delete_RoundTrips()
    {
        await using var db = NewContext();
        var service = new StockDocumentLineService(db);

        var created = await service.CreateAsync(new CreateStockDocumentLineRequest());
        Assert.True(created.IsSuccess);
        var id = created.Data;

        var detail = await service.GetByIdAsync(id);
        Assert.True(detail.IsSuccess);

        var updated = await service.UpdateAsync(id, new UpdateStockDocumentLineRequest { Id = id });
        Assert.True(updated.IsSuccess);

        var deleted = await service.DeleteAsync(id);
        Assert.True(deleted.IsSuccess);
    }

    [Fact]
    public async Task GetById_Unknown_ReturnsFailure()
    {
        await using var db = NewContext();
        var service = new StockDocumentLineService(db);
        var result = await service.GetByIdAsync(Guid.NewGuid());
        Assert.False(result.IsSuccess);
    }
}
