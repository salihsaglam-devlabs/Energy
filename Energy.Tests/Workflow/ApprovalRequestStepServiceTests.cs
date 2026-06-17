using Energy.Infrastructure.Persistence;
using Energy.Infrastructure.Workflow.ApprovalRequestStep.Services;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestStep.Requests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Energy.Tests.Workflow;

/// <summary>ApprovalRequestStep CRUD servisi round-trip testi (EF InMemory).</summary>
public sealed class ApprovalRequestStepServiceTests
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
        var service = new ApprovalRequestStepService(db);

        var created = await service.CreateAsync(new CreateApprovalRequestStepRequest());
        Assert.True(created.IsSuccess);
        var id = created.Data;

        var detail = await service.GetByIdAsync(id);
        Assert.True(detail.IsSuccess);

        var updated = await service.UpdateAsync(id, new UpdateApprovalRequestStepRequest { Id = id });
        Assert.True(updated.IsSuccess);

        var deleted = await service.DeleteAsync(id);
        Assert.True(deleted.IsSuccess);
    }

    [Fact]
    public async Task GetById_Unknown_ReturnsFailure()
    {
        await using var db = NewContext();
        var service = new ApprovalRequestStepService(db);
        var result = await service.GetByIdAsync(Guid.NewGuid());
        Assert.False(result.IsSuccess);
    }
}
