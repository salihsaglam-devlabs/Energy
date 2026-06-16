using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderMaterialPlan.Commands.DeleteWorkOrderMaterialPlan;

/// <summary>WorkOrderMaterialPlan kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteWorkOrderMaterialPlanCommand(Guid Id) : IRequest<BaseResponse<bool>>;
