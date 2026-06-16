using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialPlan.Requests;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderMaterialPlan.Commands.CreateWorkOrderMaterialPlan;

/// <summary>Yeni WorkOrderMaterialPlan oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateWorkOrderMaterialPlanCommand(CreateWorkOrderMaterialPlanRequest Request)
    : IRequest<BaseResponse<Guid>>;
