using Energy.Application.Assets.EquipmentAssignment.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentAssignment.Responses;
using MediatR;

namespace Energy.Application.Assets.EquipmentAssignment.Queries.GetEquipmentAssignmentLookup;

/// <summary>
/// <see cref="GetEquipmentAssignmentLookupQuery"/> handler'ı. <see cref="IEquipmentAssignmentLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetEquipmentAssignmentLookupQueryHandler
    : IRequestHandler<GetEquipmentAssignmentLookupQuery, BaseResponse<IReadOnlyList<EquipmentAssignmentLookupResponse>>>
{
    private readonly IEquipmentAssignmentLookupService _lookup;

    public GetEquipmentAssignmentLookupQueryHandler(IEquipmentAssignmentLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<EquipmentAssignmentLookupResponse>>> Handle(
        GetEquipmentAssignmentLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
