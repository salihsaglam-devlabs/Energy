using Energy.Infrastructure.Persistence;
using Energy.Infrastructure.Operations.WorkOrderMaterialUsage.Services;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialUsage.Requests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Energy.Tests.Operations;

/// <summary>WorkOrderMaterialUsage CRUD servisi round-trip testi (EF InMemory).</summary>
public sealed class WorkOrderMaterialUsageServiceTests
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
        var service = new WorkOrderMaterialUsageService(db);

        var created = await service.CreateAsync(new CreateWorkOrderMaterialUsageRequest());
        Assert.True(created.IsSuccess);
        var id = created.Data;

        var detail = await service.GetByIdAsync(id);
        Assert.True(detail.IsSuccess);

        var updated = await service.UpdateAsync(id, new UpdateWorkOrderMaterialUsageRequest { Id = id });
        Assert.True(updated.IsSuccess);

        var deleted = await service.DeleteAsync(id);
        Assert.True(deleted.IsSuccess);
    }

    [Fact]
    public async Task GetById_Unknown_ReturnsFailure()
    {
        await using var db = NewContext();
        var service = new WorkOrderMaterialUsageService(db);
        var result = await service.GetByIdAsync(Guid.NewGuid());
        Assert.False(result.IsSuccess);
    }
}
