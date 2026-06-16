using Energy.Infrastructure.Persistence;
using Energy.Infrastructure.Modules.ProgressPayments.Reports.ProgressPaymentSummary;
using Energy.Shared.Models.V1.ProgressPayments.Reports.ProgressPaymentSummary.Requests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Energy.Tests.Modules.ProgressPayments.Reports;

/// <summary>ProgressPaymentSummary raporu (salt-okunur) servis testi (EF InMemory).</summary>
public sealed class ProgressPaymentSummaryTests
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
        var service = new ProgressPaymentSummaryService(db);
        var result = await service.GetDataAsync(new ProgressPaymentSummaryRequest());
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Empty(result.Data!.Items);
    }

    [Fact]
    public async Task GetData_WithDateFilter_ReturnsSuccess()
    {
        await using var db = NewContext();
        var service = new ProgressPaymentSummaryService(db);
        var request = new ProgressPaymentSummaryRequest
        {
            StartDate = DateTime.UtcNow.AddYears(-1),
            EndDate = DateTime.UtcNow,
            Status = "Test",
        };
        var result = await service.GetDataAsync(request);
        Assert.True(result.IsSuccess);
    }
}
