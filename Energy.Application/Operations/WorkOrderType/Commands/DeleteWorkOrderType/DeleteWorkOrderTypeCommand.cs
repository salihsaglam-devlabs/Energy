using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderType.Commands.DeleteWorkOrderType;

/// <summary>WorkOrderType kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteWorkOrderTypeCommand(Guid Id) : IRequest<BaseResponse<bool>>;
