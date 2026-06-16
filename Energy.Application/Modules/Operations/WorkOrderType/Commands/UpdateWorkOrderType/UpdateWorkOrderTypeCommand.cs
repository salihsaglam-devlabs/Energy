using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderType.Requests;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderType.Commands.UpdateWorkOrderType;

/// <summary>Var olan WorkOrderType kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateWorkOrderTypeCommand(Guid Id, UpdateWorkOrderTypeRequest Request)
    : IRequest<BaseResponse<bool>>;
