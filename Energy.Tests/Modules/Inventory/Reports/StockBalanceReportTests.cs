using Energy.Infrastructure.Persistence;
using Energy.Infrastructure.Modules.Inventory.Reports.StockBalanceReport;
using Energy.Shared.Models.V1.Inventory.Reports.StockBalanceReport.Requests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Energy.Tests.Modules.Inventory.Reports;

/// <summary>StockBalanceReport raporu (salt-okunur) servis testi (EF InMemory).</summary>
public sealed class StockBalanceReportTests
{
    private static EnergyDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<EnergyDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new EnergyDbContext(options);
    }

    [Fact]
    public async Task GetData_Empty_ReturnsSuccessWithNoRows()
    {
        await using var db = NewContext();
        var service = new StockBalanceReportService(db);
        var result = await service.GetDataAsync(new StockBalanceReportRequest());
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Empty(result.Data!.Items);
    }

    [Fact]
    public async Task GetData_WithDateFilter_ReturnsSuccess()
    {
        await using var db = NewContext();
        var service = new StockBalanceReportService(db);
        var request = new StockBalanceReportRequest
        {
            StartDate = DateTime.UtcNow.AddYears(-1),
            EndDate = DateTime.UtcNow,
        };
        var result = await service.GetDataAsync(request);
        Assert.True(result.IsSuccess);
    }
}
