using Energy.Infrastructure.Persistence;
using Energy.Infrastructure.Modules.Finance.Reports.ReceivableAging;
using Energy.Shared.Models.V1.Finance.Reports.ReceivableAging.Requests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Energy.Tests.Modules.Finance.Reports;

/// <summary>ReceivableAging raporu (salt-okunur) servis testi (EF InMemory).</summary>
public sealed class ReceivableAgingTests
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
        var service = new ReceivableAgingService(db);
        var result = await service.GetDataAsync(new ReceivableAgingRequest());
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Empty(result.Data!.Items);
    }

    [Fact]
    public async Task GetData_WithDateFilter_ReturnsSuccess()
    {
        await using var db = NewContext();
        var service = new ReceivableAgingService(db);
        var request = new ReceivableAgingRequest
        {
            StartDate = DateTime.UtcNow.AddYears(-1),
            EndDate = DateTime.UtcNow,
        };
        var result = await service.GetDataAsync(request);
        Assert.True(result.IsSuccess);
    }
}
