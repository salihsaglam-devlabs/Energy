using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheet.Requests;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheet.Responses;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.MeasurementSheet.Queries.GetMeasurementSheetList;

/// <summary>Sayfalanmış MeasurementSheet listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetMeasurementSheetListQuery(GetMeasurementSheetListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<MeasurementSheetListResponse>>>;
