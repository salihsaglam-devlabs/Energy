using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.LocalizationResource.Requests;
using Energy.Shared.Models.V1.Core.LocalizationResource.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.LocalizationResource.Queries.GetLocalizationResourceList;

/// <summary>Sayfalanmış LocalizationResource listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetLocalizationResourceListQuery(GetLocalizationResourceListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<LocalizationResourceListResponse>>>;
