using Energy.Infrastructure.Persistence;
using Energy.Infrastructure.Modules.HR.Reports.TimesheetSummary;
using Energy.Shared.Models.V1.HR.Reports.TimesheetSummary.Requests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Energy.Tests.Modules.HR.Reports;

/// <summary>TimesheetSummary raporu (salt-okunur) servis testi (EF InMemory).</summary>
public sealed class TimesheetSummaryTests
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
        var service = new TimesheetSummaryService(db);
        var result = await service.GetDataAsync(new TimesheetSummaryRequest());
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Empty(result.Data!.Items);
    }

    [Fact]
    public async Task GetData_WithDateFilter_ReturnsSuccess()
    {
        await using var db = NewContext();
        var service = new TimesheetSummaryService(db);
        var request = new TimesheetSummaryRequest
        {
            StartDate = DateTime.UtcNow.AddYears(-1),
            EndDate = DateTime.UtcNow,
            Status = "Test",
        };
        var result = await service.GetDataAsync(request);
        Assert.True(result.IsSuccess);
    }
}
