using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.Employee.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.Employee.Queries.GetEmployeeById;

/// <summary>Kimliğe göre Employee detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetEmployeeByIdQuery(Guid Id)
    : IRequest<BaseResponse<EmployeeDetailResponse>>;
