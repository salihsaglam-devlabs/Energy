using Energy.Infrastructure.Persistence;
using Energy.Infrastructure.Organization.ExpenseClaimLine.Services;
using Energy.Shared.Models.V1.Organization.ExpenseClaimLine.Requests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Energy.Tests.Organization;

/// <summary>ExpenseClaimLine CRUD servisi round-trip testi (EF InMemory).</summary>
public sealed class ExpenseClaimLineServiceTests
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
        var service = new ExpenseClaimLineService(db);

        var created = await service.CreateAsync(new CreateExpenseClaimLineRequest());
        Assert.True(created.IsSuccess);
        var id = created.Data;

        var detail = await service.GetByIdAsync(id);
        Assert.True(detail.IsSuccess);

        var updated = await service.UpdateAsync(id, new UpdateExpenseClaimLineRequest { Id = id });
        Assert.True(updated.IsSuccess);

        var deleted = await service.DeleteAsync(id);
        Assert.True(deleted.IsSuccess);
    }

    [Fact]
    public async Task GetById_Unknown_ReturnsFailure()
    {
        await using var db = NewContext();
        var service = new ExpenseClaimLineService(db);
        var result = await service.GetByIdAsync(Guid.NewGuid());
        Assert.False(result.IsSuccess);
    }
}
