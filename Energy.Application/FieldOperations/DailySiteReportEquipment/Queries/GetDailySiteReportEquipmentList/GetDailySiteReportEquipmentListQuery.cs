using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportEquipment.Requests;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportEquipment.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.DailySiteReportEquipment.Queries.GetDailySiteReportEquipmentList;

/// <summary>Sayfalanmış DailySiteReportEquipment listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetDailySiteReportEquipmentListQuery(GetDailySiteReportEquipmentListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<DailySiteReportEquipmentListResponse>>>;
