using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Company.Requests;
using Energy.Shared.Models.V1.Core.Company.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.Company.Queries.GetCompanyList;

/// <summary>Sayfalanmış Company listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetCompanyListQuery(GetCompanyListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<CompanyListResponse>>>;
