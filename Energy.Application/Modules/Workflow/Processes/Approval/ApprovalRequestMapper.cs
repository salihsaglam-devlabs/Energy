using Energy.Shared.Models.V1.Workflow.Processes.Approval.Responses;
using ApprovalRequestEntity = Energy.Domain.Modules.Workflow.ApprovalRequest;

namespace Energy.Application.Modules.Workflow.Processes.Approval;

/// <summary>
/// <see cref="ApprovalRequestEntity"/> domain nesnesini dış <see cref="ApprovalRequestListItemResponse"/>
/// sözleşmesine dönüştüren ortak eşleyici (handler'lar arasında tek kaynak).
/// </summary>
public static class ApprovalRequestMapper
{
    public static ApprovalRequestListItemResponse Map(ApprovalRequestEntity r) => new()
    {
        Id = r.Id,
        RelatedModule = r.RelatedModule,
        RelatedEntityType = r.RelatedEntityType,
        RelatedEntityId = r.RelatedEntityId,
        Status = r.Status.ToString(),
        CurrentStepNo = r.CurrentStepNo,
        CreatedAt = r.CreatedAt,
    };
}

