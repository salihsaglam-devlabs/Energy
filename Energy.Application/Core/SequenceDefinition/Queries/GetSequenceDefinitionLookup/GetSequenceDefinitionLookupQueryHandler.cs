using Energy.Application.Core.SequenceDefinition.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.SequenceDefinition.Responses;
using MediatR;

namespace Energy.Application.Core.SequenceDefinition.Queries.GetSequenceDefinitionLookup;

/// <summary>
/// <see cref="GetSequenceDefinitionLookupQuery"/> handler'ı. <see cref="ISequenceDefinitionLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetSequenceDefinitionLookupQueryHandler
    : IRequestHandler<GetSequenceDefinitionLookupQuery, BaseResponse<IReadOnlyList<SequenceDefinitionLookupResponse>>>
{
    private readonly ISequenceDefinitionLookupService _lookup;

    public GetSequenceDefinitionLookupQueryHandler(ISequenceDefinitionLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<SequenceDefinitionLookupResponse>>> Handle(
        GetSequenceDefinitionLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
