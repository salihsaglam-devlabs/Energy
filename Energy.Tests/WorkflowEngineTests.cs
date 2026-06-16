using Energy.Shared.Common;
using Energy.Application.Workflow.Services;
using Energy.Domain.Common;
using Energy.Domain.Modules.IAM;
using Energy.Domain.Modules.Workflow;
using Energy.Infrastructure.Persistence;
using Energy.Infrastructure.Workflow.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Energy.Tests;

/// <summary>
/// Onay (workflow) motorunun davranış testleri: Sequential, ParallelAny, Quorum,
/// Reject ve Delegation. SQLite in-memory üzerinde gerçek EF Core modeliyle çalışır.
/// </summary>
public sealed class WorkflowEngineTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly AppDbContext _db;
    private readonly RecordingSourceUpdater _sourceUpdater = new();
    private readonly ApprovalWorkflowService _engine;

    public WorkflowEngineTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _db = new AppDbContext(options);
        _db.Database.EnsureCreated();

        _engine = new ApprovalWorkflowService(_db, _sourceUpdater, NullLogger<ApprovalWorkflowService>.Instance);
    }

    [Fact]
    public async Task Sequential_two_steps_completes_only_after_both_approvers()
    {
        var u1 = AddUser();
        var u2 = AddUser();
        var version = AddDefinition(
            (1, ApprovalMode.ParallelAll, null, new[] { u1 }),
            (2, ApprovalMode.ParallelAll, null, new[] { u2 }));
        await _db.SaveChangesAsync();

        var entityId = Guid.NewGuid();
        var request = await _engine.StartAsync(new StartApprovalRequest("Test", "Doc", entityId, u1));
        Assert.NotNull(request);

        // İlk adım onaylanınca süreç hâlâ beklemede olmalı.
        await _engine.ApproveAsync(request!.Id, u1);
        Assert.Equal(ApprovalRequestStatus.Pending, (await _db.ApprovalRequests.FindAsync(request.Id))!.Status);

        // İkinci adım onaylanınca süreç tamamlanır.
        await _engine.ApproveAsync(request.Id, u2);
        Assert.Equal(ApprovalRequestStatus.Approved, (await _db.ApprovalRequests.FindAsync(request.Id))!.Status);
        Assert.Equal(ApprovalOutcome.Approved, _sourceUpdater.LastOutcome);
    }

    [Fact]
    public async Task ParallelAny_completes_after_single_approval()
    {
        var u1 = AddUser();
        var u2 = AddUser();
        var version = AddDefinition((1, ApprovalMode.ParallelAny, null, new[] { u1, u2 }));
        await _db.SaveChangesAsync();

        var request = await _engine.StartAsync(new StartApprovalRequest("Test", "Doc", Guid.NewGuid(), u1));
        await _engine.ApproveAsync(request!.Id, u2);

        Assert.Equal(ApprovalRequestStatus.Approved, (await _db.ApprovalRequests.FindAsync(request.Id))!.Status);
    }

    [Fact]
    public async Task Quorum_requires_configured_count()
    {
        var u1 = AddUser();
        var u2 = AddUser();
        var u3 = AddUser();
        var version = AddDefinition((1, ApprovalMode.Quorum, 2, new[] { u1, u2, u3 }));
        await _db.SaveChangesAsync();

        var request = await _engine.StartAsync(new StartApprovalRequest("Test", "Doc", Guid.NewGuid(), u1));

        await _engine.ApproveAsync(request!.Id, u1);
        Assert.Equal(ApprovalRequestStatus.Pending, (await _db.ApprovalRequests.FindAsync(request.Id))!.Status);

        await _engine.ApproveAsync(request.Id, u2);
        Assert.Equal(ApprovalRequestStatus.Approved, (await _db.ApprovalRequests.FindAsync(request.Id))!.Status);
    }

    [Fact]
    public async Task Reject_terminates_request_as_rejected()
    {
        var u1 = AddUser();
        var version = AddDefinition((1, ApprovalMode.ParallelAll, null, new[] { u1 }));
        await _db.SaveChangesAsync();

        var request = await _engine.StartAsync(new StartApprovalRequest("Test", "Doc", Guid.NewGuid(), u1));
        await _engine.RejectAsync(request!.Id, u1, "not acceptable");

        Assert.Equal(ApprovalRequestStatus.Rejected, (await _db.ApprovalRequests.FindAsync(request.Id))!.Status);
        Assert.Equal(ApprovalOutcome.Rejected, _sourceUpdater.LastOutcome);
    }

    [Fact]
    public async Task Delegation_allows_delegate_to_approve_on_behalf()
    {
        var owner = AddUser();
        var delegateUser = AddUser();
        var version = AddDefinition((1, ApprovalMode.ParallelAll, null, new[] { owner }));

        _db.ApprovalDelegations.Add(new ApprovalDelegation
        {
            Id = Guid.NewGuid(),
            DelegatorUserId = owner,
            DelegateUserId = delegateUser,
            StartDate = DateTime.UtcNow.AddDays(-1),
            EndDate = DateTime.UtcNow.AddDays(1),
            IsActive = true,
        });
        await _db.SaveChangesAsync();

        var request = await _engine.StartAsync(new StartApprovalRequest("Test", "Doc", Guid.NewGuid(), owner));

        // Devralan kullanıcı, asıl onaycı adına onaylayabilir.
        await _engine.ApproveAsync(request!.Id, delegateUser);

        Assert.Equal(ApprovalRequestStatus.Approved, (await _db.ApprovalRequests.FindAsync(request.Id))!.Status);
    }

    // ---- Yardımcılar ----

    private Guid AddUser()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "u" + Guid.NewGuid().ToString("N")[..8],
            Email = Guid.NewGuid().ToString("N") + "@test.local",
            FirstName = "Test",
            LastName = "User",
            PasswordHash = "x",
            IsActive = true,
        };
        _db.Users.Add(user);
        return user.Id;
    }

    private ApprovalDefinitionVersion AddDefinition(
        params (int StepNo, ApprovalMode Mode, int? RequiredCount, Guid[] ApproverUserIds)[] steps)
    {
        var definition = new ApprovalDefinition
        {
            Id = Guid.NewGuid(),
            Code = "TEST-" + Guid.NewGuid().ToString("N")[..6],
            Name = "Test",
            RelatedModule = "Test",
            RelatedEntityType = "Doc",
            IsActive = true,
        };
        _db.ApprovalDefinitions.Add(definition);

        var version = new ApprovalDefinitionVersion
        {
            Id = Guid.NewGuid(),
            ApprovalDefinitionId = definition.Id,
            VersionNo = 1,
            EffectiveFrom = DateTime.UtcNow.AddDays(-1),
            IsActive = true,
        };
        _db.ApprovalDefinitionVersions.Add(version);

        foreach (var (stepNo, mode, requiredCount, approverIds) in steps)
        {
            var step = new ApprovalStepDefinition
            {
                Id = Guid.NewGuid(),
                ApprovalDefinitionVersionId = version.Id,
                StepNo = stepNo,
                Name = "Step " + stepNo,
                ApprovalMode = mode,
                RequiredApprovalCount = requiredCount,
                IsRequired = true,
            };
            _db.ApprovalStepDefinitions.Add(step);

            foreach (var approverId in approverIds)
            {
                _db.ApprovalStepApprovers.Add(new ApprovalStepApprover
                {
                    Id = Guid.NewGuid(),
                    ApprovalStepDefinitionId = step.Id,
                    ApproverType = ApproverType.User,
                    ApproverUserId = approverId,
                });
            }
        }

        return version;
    }

    public void Dispose()
    {
        _db.Dispose();
        _connection.Dispose();
    }

    /// <summary>Test için kaynak güncelleyici stub'ı — son sonucu kaydeder.</summary>
    private sealed class RecordingSourceUpdater : IApprovalSourceUpdater
    {
        public ApprovalOutcome? LastOutcome { get; private set; }

        public Task ApplyAsync(string relatedModule, string relatedEntityType, Guid entityId,
            Guid approvalRequestId, ApprovalOutcome outcome, CancellationToken ct = default)
        {
            LastOutcome = outcome;
            return Task.CompletedTask;
        }
    }
}

