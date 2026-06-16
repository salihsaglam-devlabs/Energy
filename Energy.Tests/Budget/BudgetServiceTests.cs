using BudgetEntity = Energy.Domain.Budget.Budget;
using Energy.Infrastructure.Persistence;
using Energy.Infrastructure.Budget.Budget.Services;
using Energy.Shared.Models.V1.Budget.Budget.Requests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Energy.Tests.Budget;

/// <summary>BudgetEntity CRUD servisi round-trip testi (EF InMemory).</summary>
public sealed class BudgetServiceTests
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
        var service = new BudgetService(db);

        var created = await service.CreateAsync(new CreateBudgetRequest());
        Assert.True(created.IsSuccess);
        var id = created.Data;

        var detail = await service.GetByIdAsync(id);
        Assert.True(detail.IsSuccess);

        var updated = await service.UpdateAsync(id, new UpdateBudgetRequest { Id = id });
        Assert.True(updated.IsSuccess);

        var deleted = await service.DeleteAsync(id);
        Assert.True(deleted.IsSuccess);
    }

    [Fact]
    public async Task GetById_Unknown_ReturnsFailure()
    {
        await using var db = NewContext();
        var service = new BudgetService(db);
        var result = await service.GetByIdAsync(Guid.NewGuid());
        Assert.False(result.IsSuccess);
    }
}
