using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Department.Requests;
using Energy.Shared.Models.V1.Core.Department.Responses;
using MediatR;

namespace Energy.Application.Core.Department.Queries.GetDepartmentList;

/// <summary>Sayfalanmış Department listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetDepartmentListQuery(GetDepartmentListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<DepartmentListResponse>>>;
