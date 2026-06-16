using Energy.Infrastructure.Persistence;
using Energy.Infrastructure.Modules.Procurement.Reports.PurchaseOrderSummary;
using Energy.Shared.Models.V1.Procurement.Reports.PurchaseOrderSummary.Requests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Energy.Tests.Modules.Procurement.Reports;

/// <summary>PurchaseOrderSummary raporu (salt-okunur) servis testi (EF InMemory).</summary>
public sealed class PurchaseOrderSummaryTests
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
        var service = new PurchaseOrderSummaryService(db);
        var result = await service.GetDataAsync(new PurchaseOrderSummaryRequest());
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Empty(result.Data!.Items);
    }

    [Fact]
    public async Task GetData_WithDateFilter_ReturnsSuccess()
    {
        await using var db = NewContext();
        var service = new PurchaseOrderSummaryService(db);
        var request = new PurchaseOrderSummaryRequest
        {
            StartDate = DateTime.UtcNow.AddYears(-1),
            EndDate = DateTime.UtcNow,
            Status = "Test",
        };
        var result = await service.GetDataAsync(request);
        Assert.True(result.IsSuccess);
    }
}
