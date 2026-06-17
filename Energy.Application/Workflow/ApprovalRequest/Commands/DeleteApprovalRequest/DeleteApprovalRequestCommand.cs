using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Workflow.ApprovalRequest.Commands.DeleteApprovalRequest;

/// <summary>ApprovalRequest kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteApprovalRequestCommand(Guid Id) : IRequest<BaseResponse<bool>>;
