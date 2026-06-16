using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.LeaveRequest.Commands.DeleteLeaveRequest;

/// <summary>LeaveRequest kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteLeaveRequestCommand(Guid Id) : IRequest<BaseResponse<bool>>;
