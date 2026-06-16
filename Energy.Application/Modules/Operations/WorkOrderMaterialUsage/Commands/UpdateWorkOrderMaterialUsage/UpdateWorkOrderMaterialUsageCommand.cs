using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialUsage.Requests;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderMaterialUsage.Commands.UpdateWorkOrderMaterialUsage;

/// <summary>Var olan WorkOrderMaterialUsage kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateWorkOrderMaterialUsageCommand(Guid Id, UpdateWorkOrderMaterialUsageRequest Request)
    : IRequest<BaseResponse<bool>>;
