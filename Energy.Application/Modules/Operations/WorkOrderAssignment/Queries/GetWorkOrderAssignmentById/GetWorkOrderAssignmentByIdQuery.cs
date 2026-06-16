using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderAssignment.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderAssignment.Queries.GetWorkOrderAssignmentById;

/// <summary>Kimliğe göre WorkOrderAssignment detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetWorkOrderAssignmentByIdQuery(Guid Id)
    : IRequest<BaseResponse<WorkOrderAssignmentDetailResponse>>;
