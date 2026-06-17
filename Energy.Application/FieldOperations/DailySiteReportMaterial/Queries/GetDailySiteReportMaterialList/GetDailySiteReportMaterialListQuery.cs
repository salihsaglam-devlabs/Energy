using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportMaterial.Requests;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportMaterial.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.DailySiteReportMaterial.Queries.GetDailySiteReportMaterialList;

/// <summary>Sayfalanmış DailySiteReportMaterial listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetDailySiteReportMaterialListQuery(GetDailySiteReportMaterialListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<DailySiteReportMaterialListResponse>>>;
