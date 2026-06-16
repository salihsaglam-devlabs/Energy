using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.Employee.Requests;
using Energy.Shared.Models.V1.Organization.Employee.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.Employee.Queries.GetEmployeeList;

/// <summary>Sayfalanmış Employee listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetEmployeeListQuery(GetEmployeeListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<EmployeeListResponse>>>;
