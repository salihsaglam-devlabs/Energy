using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentAssignment.Responses;
using MediatR;

namespace Energy.Application.Assets.EquipmentAssignment.Queries.GetEquipmentAssignmentById;

/// <summary>Kimliğe göre EquipmentAssignment detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetEquipmentAssignmentByIdQuery(Guid Id)
    : IRequest<BaseResponse<EquipmentAssignmentDetailResponse>>;
