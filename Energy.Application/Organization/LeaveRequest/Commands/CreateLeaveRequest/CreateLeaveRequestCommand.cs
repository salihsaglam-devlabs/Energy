using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.LeaveRequest.Requests;
using MediatR;

namespace Energy.Application.Organization.LeaveRequest.Commands.CreateLeaveRequest;

/// <summary>Yeni LeaveRequest oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateLeaveRequestCommand(CreateLeaveRequestRequest Request)
    : IRequest<BaseResponse<Guid>>;
