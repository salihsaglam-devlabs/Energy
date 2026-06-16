using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportEquipment.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.DailySiteReportEquipment.Queries.GetDailySiteReportEquipmentById;

/// <summary>Kimliğe göre DailySiteReportEquipment detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetDailySiteReportEquipmentByIdQuery(Guid Id)
    : IRequest<BaseResponse<DailySiteReportEquipmentDetailResponse>>;
