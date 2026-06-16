using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportMaterial.Responses;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.DailySiteReportMaterial.Queries.GetDailySiteReportMaterialById;

/// <summary>Kimliğe göre DailySiteReportMaterial detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetDailySiteReportMaterialByIdQuery(Guid Id)
    : IRequest<BaseResponse<DailySiteReportMaterialDetailResponse>>;
