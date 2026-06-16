using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderStatusHistory.Commands.DeleteWorkOrderStatusHistory;

/// <summary>WorkOrderStatusHistory kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteWorkOrderStatusHistoryCommand(Guid Id) : IRequest<BaseResponse<bool>>;
