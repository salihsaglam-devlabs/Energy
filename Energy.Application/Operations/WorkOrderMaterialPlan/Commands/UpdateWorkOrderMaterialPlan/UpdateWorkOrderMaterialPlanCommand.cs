using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialPlan.Requests;
using MediatR;

namespace Energy.Application.Operations.WorkOrderMaterialPlan.Commands.UpdateWorkOrderMaterialPlan;

/// <summary>Var olan WorkOrderMaterialPlan kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateWorkOrderMaterialPlanCommand(Guid Id, UpdateWorkOrderMaterialPlanRequest Request)
    : IRequest<BaseResponse<bool>>;
