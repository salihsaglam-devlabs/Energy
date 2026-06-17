using Energy.Infrastructure.Persistence;
using Energy.Infrastructure.Finance.Reports.PayableAging;
using Energy.Shared.Models.V1.Finance.Reports.PayableAging.Requests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Energy.Tests.Finance.Reports;

/// <summary>PayableAging raporu (salt-okunur) servis testi (EF InMemory).</summary>
public sealed class PayableAgingTests
{
    private static AppDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetData_Empty_ReturnsSuccessWithNoRows()
    {
        await using var db = NewContext();
        var service = new PayableAgingService(db);
        var result = await service.GetDataAsync(new PayableAgingRequest());
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Empty(result.Data!.Items);
    }

    [Fact]
    public async Task GetData_WithDateFilter_ReturnsSuccess()
    {
        await using var db = NewContext();
        var service = new PayableAgingService(db);
        var request = new PayableAgingRequest
        {
            StartDate = DateTime.UtcNow.AddYears(-1),
            EndDate = DateTime.UtcNow,
        };
        var result = await service.GetDataAsync(request);
        Assert.True(result.IsSuccess);
    }
}
