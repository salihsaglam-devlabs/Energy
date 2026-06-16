using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrder.Commands.DeleteWorkOrder;

/// <summary>WorkOrder kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteWorkOrderCommand(Guid Id) : IRequest<BaseResponse<bool>>;
