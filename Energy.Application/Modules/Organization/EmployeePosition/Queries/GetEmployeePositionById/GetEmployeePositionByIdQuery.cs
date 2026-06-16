using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeePosition.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.EmployeePosition.Queries.GetEmployeePositionById;

/// <summary>Kimliğe göre EmployeePosition detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetEmployeePositionByIdQuery(Guid Id)
    : IRequest<BaseResponse<EmployeePositionDetailResponse>>;
