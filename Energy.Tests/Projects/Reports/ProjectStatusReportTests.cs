using Energy.Infrastructure.Persistence;
using Energy.Infrastructure.Projects.Reports.ProjectStatusReport;
using Energy.Shared.Models.V1.Projects.Reports.ProjectStatusReport.Requests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Energy.Tests.Projects.Reports;

/// <summary>ProjectStatusReport raporu (salt-okunur) servis testi (EF InMemory).</summary>
public sealed class ProjectStatusReportTests
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
        var service = new ProjectStatusReportService(db);
        var result = await service.GetDataAsync(new ProjectStatusReportRequest());
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Empty(result.Data!.Items);
    }

    [Fact]
    public async Task GetData_WithDateFilter_ReturnsSuccess()
    {
        await using var db = NewContext();
        var service = new ProjectStatusReportService(db);
        var request = new ProjectStatusReportRequest
        {
            StartDate = DateTime.UtcNow.AddYears(-1),
            EndDate = DateTime.UtcNow,
        };
        var result = await service.GetDataAsync(request);
        Assert.True(result.IsSuccess);
    }
}
