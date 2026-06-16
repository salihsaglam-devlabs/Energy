using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentAssignment.Requests;
using Energy.Shared.Models.V1.Assets.EquipmentAssignment.Responses;
using MediatR;

namespace Energy.Application.Assets.EquipmentAssignment.Queries.GetEquipmentAssignmentList;

/// <summary>Sayfalanmış EquipmentAssignment listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetEquipmentAssignmentListQuery(GetEquipmentAssignmentListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<EquipmentAssignmentListResponse>>>;
