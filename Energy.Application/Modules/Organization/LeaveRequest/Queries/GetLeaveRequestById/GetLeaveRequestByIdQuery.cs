using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.LeaveRequest.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.LeaveRequest.Queries.GetLeaveRequestById;

/// <summary>Kimliğe göre LeaveRequest detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetLeaveRequestByIdQuery(Guid Id)
    : IRequest<BaseResponse<LeaveRequestDetailResponse>>;
