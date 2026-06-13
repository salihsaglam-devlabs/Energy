using Energy.Application.Workflow.Services;
using Energy.Domain.Common;
using Energy.Domain.Identity;
using Energy.Domain.Notifications;
using Energy.Domain.Workflow;
using Energy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Energy.Infrastructure.Workflow.Services;

/// <summary>
/// <see cref="IApprovalWorkflowService"/>'in EF Core uygulaması. Dinamik onay akışı
/// motoru; tüm onay/ret/iade/iptal işlemlerini transaction içinde, snapshot onaycılar
/// ve mod kurallarıyla (Sequential / ParallelAny / ParallelAll / Quorum) yürütür.
/// </summary>
public sealed class ApprovalWorkflowService : IApprovalWorkflowService
{
    private readonly AppDbContext _db;
    private readonly IApprovalSourceUpdater _sourceUpdater;
    private readonly ILogger<ApprovalWorkflowService> _logger;

    public ApprovalWorkflowService(
        AppDbContext db,
        IApprovalSourceUpdater sourceUpdater,
        ILogger<ApprovalWorkflowService> logger)
    {
        _db = db;
        _sourceUpdater = sourceUpdater;
        _logger = logger;
    }

    public async Task<ApprovalRequest?> StartAsync(StartApprovalRequest request, CancellationToken ct = default)
    {
        var version = await SelectActiveVersionAsync(request.RelatedModule, request.RelatedEntityType, request.Fields, ct);
        if (version is null)
        {
            _logger.LogInformation(
                "No active approval definition for {Module}/{Entity}; skipping workflow.",
                request.RelatedModule, request.RelatedEntityType);
            return null;
        }

        var stepDefs = await _db.ApprovalStepDefinitions
            .Where(s => s.ApprovalDefinitionVersionId == version.Id)
            .OrderBy(s => s.StepNo)
            .ToListAsync(ct);

        if (stepDefs.Count == 0)
        {
            _logger.LogWarning("Approval version {Version} has no steps; skipping workflow.", version.Id);
            return null;
        }

        await using var tx = await _db.Database.BeginTransactionAsync(ct);

        var approvalRequest = new ApprovalRequest
        {
            Id = Guid.NewGuid(),
            ApprovalDefinitionVersionId = version.Id,
            RelatedModule = request.RelatedModule,
            RelatedEntityType = request.RelatedEntityType,
            RelatedEntityId = request.RelatedEntityId,
            RequestedByUserId = request.RequestedByUserId,
            Status = ApprovalRequestStatus.Pending,
            CurrentStepNo = stepDefs[0].StepNo,
        };
        _db.ApprovalRequests.Add(approvalRequest);

        foreach (var def in stepDefs)
        {
            _db.ApprovalRequestSteps.Add(new ApprovalRequestStep
            {
                Id = Guid.NewGuid(),
                ApprovalRequestId = approvalRequest.Id,
                ApprovalStepDefinitionId = def.Id,
                StepNo = def.StepNo,
                ApprovalMode = def.ApprovalMode,
                RequiredApprovalCount = def.RequiredApprovalCount,
                Status = ApprovalStepStatus.Waiting,
            });
        }

        await _db.SaveChangesAsync(ct);

        // İlk adımı aktive et (snapshot onaycılar + bildirimler).
        var firstStep = await _db.ApprovalRequestSteps
            .Where(s => s.ApprovalRequestId == approvalRequest.Id)
            .OrderBy(s => s.StepNo)
            .FirstAsync(ct);
        await ActivateStepAsync(approvalRequest, firstStep, ct);

        await _sourceUpdater.ApplyAsync(
            request.RelatedModule, request.RelatedEntityType, request.RelatedEntityId,
            approvalRequest.Id, ApprovalOutcome.Pending, ct);

        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);

        return approvalRequest;
    }

    public async Task<ApprovalRequest> ApproveAsync(Guid approvalRequestId, Guid actingUserId, string? note = null, CancellationToken ct = default)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(ct);
        var request = await LoadPendingRequestAsync(approvalRequestId, ct);

        var activeSteps = await _db.ApprovalRequestSteps
            .Where(s => s.ApprovalRequestId == request.Id && s.Status == ApprovalStepStatus.Active)
            .ToListAsync(ct);

        var approver = await ResolveActingApproverAsync(activeSteps.Select(s => s.Id).ToList(), actingUserId, ct);
        if (approver is null)
        {
            throw new InvalidOperationException("No pending approval is assigned to the acting user for this request.");
        }

        var now = DateTime.UtcNow;
        approver.Status = ApprovalApproverStatus.Approved;
        approver.ActionAt = now;

        _db.ApprovalActions.Add(new ApprovalAction
        {
            Id = Guid.NewGuid(),
            ApprovalRequestId = request.Id,
            ApprovalRequestStepId = approver.ApprovalRequestStepId,
            UserId = actingUserId,
            ActionType = ApprovalActionType.Approve,
            ActionAt = now,
            Note = note,
        });
        await _db.SaveChangesAsync(ct);

        var step = activeSteps.First(s => s.Id == approver.ApprovalRequestStepId);
        if (await IsStepCompletedAsync(step, ct))
        {
            step.Status = ApprovalStepStatus.Approved;
            await _db.SaveChangesAsync(ct);

            var nextStep = await _db.ApprovalRequestSteps
                .Where(s => s.ApprovalRequestId == request.Id && s.Status == ApprovalStepStatus.Waiting && s.StepNo > step.StepNo)
                .OrderBy(s => s.StepNo)
                .FirstOrDefaultAsync(ct);

            if (nextStep is null)
            {
                await CompleteRequestAsync(request, ApprovalRequestStatus.Approved, ApprovalOutcome.Approved, ct);
            }
            else
            {
                request.CurrentStepNo = nextStep.StepNo;
                await ActivateStepAsync(request, nextStep, ct);
                await _db.SaveChangesAsync(ct);
            }
        }

        await tx.CommitAsync(ct);
        return request;
    }

    public Task<ApprovalRequest> RejectAsync(Guid approvalRequestId, Guid actingUserId, string? note = null, CancellationToken ct = default)
        => TerminateAsync(approvalRequestId, actingUserId, ApprovalActionType.Reject, ApprovalRequestStatus.Rejected, ApprovalOutcome.Rejected, note, ct);

    public Task<ApprovalRequest> ReturnAsync(Guid approvalRequestId, Guid actingUserId, string? note = null, CancellationToken ct = default)
        => TerminateAsync(approvalRequestId, actingUserId, ApprovalActionType.Return, ApprovalRequestStatus.Returned, ApprovalOutcome.Returned, note, ct);

    public Task<ApprovalRequest> CancelAsync(Guid approvalRequestId, Guid actingUserId, string? note = null, CancellationToken ct = default)
        => TerminateAsync(approvalRequestId, actingUserId, ApprovalActionType.Cancel, ApprovalRequestStatus.Cancelled, ApprovalOutcome.Cancelled, note, ct);

    public async Task<IReadOnlyList<ApprovalRequest>> GetPendingForUserAsync(Guid userId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        // Doğrudan atanan veya delegasyonla devralınan bekleyen onaylar.
        var delegatorIds = await _db.ApprovalDelegations
            .Where(d => d.DelegateUserId == userId && d.IsActive && d.StartDate <= now && d.EndDate >= now)
            .Select(d => d.DelegatorUserId)
            .ToListAsync(ct);

        var candidateUserIds = delegatorIds.Append(userId).Distinct().ToList();

        var requestIds = await (
            from approver in _db.ApprovalRequestApprovers
            join step in _db.ApprovalRequestSteps on approver.ApprovalRequestStepId equals step.Id
            join req in _db.ApprovalRequests on step.ApprovalRequestId equals req.Id
            where approver.Status == ApprovalApproverStatus.Waiting
                  && step.Status == ApprovalStepStatus.Active
                  && req.Status == ApprovalRequestStatus.Pending
                  && candidateUserIds.Contains(approver.UserId)
            select req.Id)
            .Distinct()
            .ToListAsync(ct);

        return await _db.ApprovalRequests.Where(r => requestIds.Contains(r.Id)).ToListAsync(ct);
    }

    // ---- Yardımcılar ----

    private async Task<ApprovalRequest> TerminateAsync(
        Guid approvalRequestId, Guid actingUserId, ApprovalActionType action,
        ApprovalRequestStatus status, ApprovalOutcome outcome, string? note, CancellationToken ct)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(ct);
        var request = await LoadPendingRequestAsync(approvalRequestId, ct);

        var now = DateTime.UtcNow;

        // İlgili onaycı kaydı varsa durumunu işaretle (ret için).
        var activeStepIds = await _db.ApprovalRequestSteps
            .Where(s => s.ApprovalRequestId == request.Id && s.Status == ApprovalStepStatus.Active)
            .Select(s => s.Id)
            .ToListAsync(ct);
        var approver = await ResolveActingApproverAsync(activeStepIds, actingUserId, ct);
        if (approver is not null && action == ApprovalActionType.Reject)
        {
            approver.Status = ApprovalApproverStatus.Rejected;
            approver.ActionAt = now;
        }

        // Açık adımları kapat.
        var openSteps = await _db.ApprovalRequestSteps
            .Where(s => s.ApprovalRequestId == request.Id &&
                        (s.Status == ApprovalStepStatus.Active || s.Status == ApprovalStepStatus.Waiting))
            .ToListAsync(ct);
        foreach (var s in openSteps)
        {
            s.Status = action == ApprovalActionType.Reject ? ApprovalStepStatus.Rejected : ApprovalStepStatus.Skipped;
        }

        _db.ApprovalActions.Add(new ApprovalAction
        {
            Id = Guid.NewGuid(),
            ApprovalRequestId = request.Id,
            ApprovalRequestStepId = approver?.ApprovalRequestStepId,
            UserId = actingUserId,
            ActionType = action,
            ActionAt = now,
            Note = note,
        });

        await CompleteRequestAsync(request, status, outcome, ct);
        await tx.CommitAsync(ct);
        return request;
    }

    private async Task<ApprovalRequest> LoadPendingRequestAsync(Guid id, CancellationToken ct)
    {
        var request = await _db.ApprovalRequests.FirstOrDefaultAsync(r => r.Id == id, ct)
            ?? throw new InvalidOperationException($"Approval request {id} not found.");
        if (request.Status != ApprovalRequestStatus.Pending)
        {
            throw new InvalidOperationException($"Approval request {id} is not pending (status: {request.Status}).");
        }
        return request;
    }

    private async Task<ApprovalDefinitionVersion?> SelectActiveVersionAsync(
        string module, string entityType, IReadOnlyDictionary<string, string>? fields, CancellationToken ct)
    {
        var now = DateTime.UtcNow;

        var definitionIds = await _db.ApprovalDefinitions
            .Where(d => d.RelatedModule == module && d.RelatedEntityType == entityType && d.IsActive)
            .Select(d => d.Id)
            .ToListAsync(ct);

        if (definitionIds.Count == 0)
        {
            return null;
        }

        var versions = await _db.ApprovalDefinitionVersions
            .Where(v => definitionIds.Contains(v.ApprovalDefinitionId)
                        && v.IsActive
                        && v.EffectiveFrom <= now
                        && (v.EffectiveTo == null || v.EffectiveTo >= now))
            .OrderByDescending(v => v.VersionNo)
            .ToListAsync(ct);

        ApprovalDefinitionVersion? fallback = null;
        foreach (var version in versions)
        {
            var conditions = await _db.ApprovalConditions
                .Where(c => c.ApprovalDefinitionVersionId == version.Id)
                .ToListAsync(ct);

            if (conditions.Count == 0)
            {
                fallback ??= version;
                continue;
            }

            if (conditions.All(c => EvaluateCondition(c, fields)))
            {
                return version; // Koşullu eşleşme daha özeldir; önceliklidir.
            }
        }

        return fallback;
    }

    private static bool EvaluateCondition(ApprovalCondition condition, IReadOnlyDictionary<string, string>? fields)
    {
        if (fields is null || !fields.TryGetValue(condition.FieldName, out var actual))
        {
            return false;
        }

        switch (condition.Operator)
        {
            case ConditionOperator.Equals:
                return string.Equals(actual, condition.ValueText, StringComparison.OrdinalIgnoreCase);
            case ConditionOperator.NotEquals:
                return !string.Equals(actual, condition.ValueText, StringComparison.OrdinalIgnoreCase);
            case ConditionOperator.In:
                return (condition.ValueText ?? string.Empty)
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Any(v => string.Equals(v, actual, StringComparison.OrdinalIgnoreCase));
            case ConditionOperator.GreaterThan:
            case ConditionOperator.GreaterThanOrEqual:
            case ConditionOperator.LessThan:
            case ConditionOperator.LessThanOrEqual:
                if (!decimal.TryParse(actual, out var actualNumber) || condition.ValueNumber is null)
                {
                    return false;
                }
                var bound = condition.ValueNumber.Value;
                return condition.Operator switch
                {
                    ConditionOperator.GreaterThan => actualNumber > bound,
                    ConditionOperator.GreaterThanOrEqual => actualNumber >= bound,
                    ConditionOperator.LessThan => actualNumber < bound,
                    ConditionOperator.LessThanOrEqual => actualNumber <= bound,
                    _ => false,
                };
            default:
                return false;
        }
    }

    private async Task ActivateStepAsync(ApprovalRequest request, ApprovalRequestStep step, CancellationToken ct)
    {
        step.Status = ApprovalStepStatus.Active;

        var approverDefs = await _db.ApprovalStepApprovers
            .Where(a => a.ApprovalStepDefinitionId == step.ApprovalStepDefinitionId)
            .ToListAsync(ct);

        var userIds = new HashSet<Guid>();
        foreach (var def in approverDefs)
        {
            switch (def.ApproverType)
            {
                case ApproverType.User when def.ApproverUserId is not null:
                    userIds.Add(def.ApproverUserId.Value);
                    break;
                case ApproverType.Role when def.ApproverRoleId is not null:
                    var roleUsers = await _db.UserRoles
                        .Where(ur => ur.RoleId == def.ApproverRoleId.Value)
                        .Select(ur => ur.UserId)
                        .ToListAsync(ct);
                    foreach (var u in roleUsers) userIds.Add(u);
                    break;
                case ApproverType.DepartmentManager when def.ApproverDepartmentId is not null:
                    var managerId = await _db.Departments
                        .Where(d => d.Id == def.ApproverDepartmentId.Value)
                        .Select(d => d.ManagerUserId)
                        .FirstOrDefaultAsync(ct);
                    if (managerId is not null) userIds.Add(managerId.Value);
                    break;
            }
        }

        foreach (var userId in userIds)
        {
            _db.ApprovalRequestApprovers.Add(new ApprovalRequestApprover
            {
                Id = Guid.NewGuid(),
                ApprovalRequestStepId = step.Id,
                UserId = userId,
                Status = ApprovalApproverStatus.Waiting,
            });
        }

        if (userIds.Count > 0)
        {
            await NotifyApproversAsync(request, userIds, ct);
        }

        await _db.SaveChangesAsync(ct);
    }

    private async Task NotifyApproversAsync(ApprovalRequest request, IEnumerable<Guid> userIds, CancellationToken ct)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            Title = "Notifications.PendingApproval.Title",
            Body = "Notifications.PendingApproval.Body",
            NotificationType = "PendingApproval",
            RelatedModule = request.RelatedModule,
            RelatedEntityType = request.RelatedEntityType,
            RelatedEntityId = request.RelatedEntityId,
        };
        _db.Notifications.Add(notification);

        foreach (var userId in userIds)
        {
            _db.NotificationRecipients.Add(new NotificationRecipient
            {
                Id = Guid.NewGuid(),
                NotificationId = notification.Id,
                UserId = userId,
                IsRead = false,
            });
        }
    }

    private async Task<bool> IsStepCompletedAsync(ApprovalRequestStep step, CancellationToken ct)
    {
        var approvers = await _db.ApprovalRequestApprovers
            .Where(a => a.ApprovalRequestStepId == step.Id)
            .ToListAsync(ct);

        if (approvers.Count == 0)
        {
            return true; // Onaycı atanmamış adım otomatik tamamlanır.
        }

        var approved = approvers.Count(a => a.Status == ApprovalApproverStatus.Approved);

        return step.ApprovalMode switch
        {
            ApprovalMode.ParallelAny => approved >= 1,
            ApprovalMode.Quorum => approved >= Math.Max(1, step.RequiredApprovalCount ?? 1),
            // ParallelAll ve Sequential: tüm onaycıların onayı gerekir.
            _ => approved == approvers.Count,
        };
    }

    private async Task<ApprovalRequestApprover?> ResolveActingApproverAsync(
        IReadOnlyCollection<Guid> activeStepIds, Guid actingUserId, CancellationToken ct)
    {
        if (activeStepIds.Count == 0)
        {
            return null;
        }

        var waiting = await _db.ApprovalRequestApprovers
            .Where(a => activeStepIds.Contains(a.ApprovalRequestStepId) && a.Status == ApprovalApproverStatus.Waiting)
            .ToListAsync(ct);

        // Doğrudan atanmış onaycı.
        var direct = waiting.FirstOrDefault(a => a.UserId == actingUserId);
        if (direct is not null)
        {
            return direct;
        }

        // Delegasyonla devralınan onaycı.
        var now = DateTime.UtcNow;
        foreach (var candidate in waiting)
        {
            var delegated = await _db.ApprovalDelegations.AnyAsync(
                d => d.DelegatorUserId == candidate.UserId
                     && d.DelegateUserId == actingUserId
                     && d.IsActive && d.StartDate <= now && d.EndDate >= now, ct);
            if (delegated)
            {
                candidate.DelegatedFromUserId = candidate.UserId;
                return candidate;
            }
        }

        return null;
    }

    private async Task CompleteRequestAsync(
        ApprovalRequest request, ApprovalRequestStatus status, ApprovalOutcome outcome, CancellationToken ct)
    {
        request.Status = status;
        await _db.SaveChangesAsync(ct);

        await _sourceUpdater.ApplyAsync(
            request.RelatedModule, request.RelatedEntityType, request.RelatedEntityId,
            request.Id, outcome, ct);

        await _db.SaveChangesAsync(ct);
    }
}

