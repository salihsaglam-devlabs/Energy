using Energy.Application.FieldOperations.ProgressEntry.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.ProgressEntry.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.ProgressEntry.Queries.GetProgressEntryLookup;

/// <summary>
/// <see cref="GetProgressEntryLookupQuery"/> handler'ı. <see cref="IProgressEntryLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetProgressEntryLookupQueryHandler
    : IRequestHandler<GetProgressEntryLookupQuery, BaseResponse<IReadOnlyList<ProgressEntryLookupResponse>>>
{
    private readonly IProgressEntryLookupService _lookup;

    public GetProgressEntryLookupQueryHandler(IProgressEntryLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ProgressEntryLookupResponse>>> Handle(
        GetProgressEntryLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
