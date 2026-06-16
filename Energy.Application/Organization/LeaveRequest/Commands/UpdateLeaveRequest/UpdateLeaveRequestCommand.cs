using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.LeaveRequest.Requests;
using MediatR;

namespace Energy.Application.Organization.LeaveRequest.Commands.UpdateLeaveRequest;

/// <summary>Var olan LeaveRequest kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateLeaveRequestCommand(Guid Id, UpdateLeaveRequestRequest Request)
    : IRequest<BaseResponse<bool>>;
