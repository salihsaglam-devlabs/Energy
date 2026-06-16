using Energy.Application.Modules.Operations.WorkOrderAssignment.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderAssignment.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderAssignment.Queries.GetWorkOrderAssignmentLookup;

/// <summary>
/// <see cref="GetWorkOrderAssignmentLookupQuery"/> handler'ı. <see cref="IWorkOrderAssignmentLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetWorkOrderAssignmentLookupQueryHandler
    : IRequestHandler<GetWorkOrderAssignmentLookupQuery, BaseResponse<IReadOnlyList<WorkOrderAssignmentLookupResponse>>>
{
    private readonly IWorkOrderAssignmentLookupService _lookup;

    public GetWorkOrderAssignmentLookupQueryHandler(IWorkOrderAssignmentLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<WorkOrderAssignmentLookupResponse>>> Handle(
        GetWorkOrderAssignmentLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
