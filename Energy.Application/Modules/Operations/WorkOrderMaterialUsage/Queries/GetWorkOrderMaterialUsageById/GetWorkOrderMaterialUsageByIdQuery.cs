using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialUsage.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderMaterialUsage.Queries.GetWorkOrderMaterialUsageById;

/// <summary>Kimliğe göre WorkOrderMaterialUsage detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetWorkOrderMaterialUsageByIdQuery(Guid Id)
    : IRequest<BaseResponse<WorkOrderMaterialUsageDetailResponse>>;
