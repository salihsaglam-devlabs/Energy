using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderChecklist.Commands.DeleteWorkOrderChecklist;

/// <summary>WorkOrderChecklist kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteWorkOrderChecklistCommand(Guid Id) : IRequest<BaseResponse<bool>>;
