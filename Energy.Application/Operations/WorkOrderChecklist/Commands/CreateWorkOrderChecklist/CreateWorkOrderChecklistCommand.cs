using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklist.Requests;
using MediatR;

namespace Energy.Application.Operations.WorkOrderChecklist.Commands.CreateWorkOrderChecklist;

/// <summary>Yeni WorkOrderChecklist oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateWorkOrderChecklistCommand(CreateWorkOrderChecklistRequest Request)
    : IRequest<BaseResponse<Guid>>;
