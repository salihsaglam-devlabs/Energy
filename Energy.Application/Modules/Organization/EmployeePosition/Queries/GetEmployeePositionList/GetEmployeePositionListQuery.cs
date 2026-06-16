using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeePosition.Requests;
using Energy.Shared.Models.V1.Organization.EmployeePosition.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.EmployeePosition.Queries.GetEmployeePositionList;

/// <summary>Sayfalanmış EmployeePosition listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetEmployeePositionListQuery(GetEmployeePositionListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<EmployeePositionListResponse>>>;
