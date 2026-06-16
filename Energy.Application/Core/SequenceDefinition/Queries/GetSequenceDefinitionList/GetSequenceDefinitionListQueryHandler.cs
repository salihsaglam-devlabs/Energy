using Energy.Application.Core.SequenceDefinition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.SequenceDefinition.Responses;
using MediatR;

namespace Energy.Application.Core.SequenceDefinition.Queries.GetSequenceDefinitionList;

/// <summary>
/// <see cref="GetSequenceDefinitionListQuery"/> handler'ı. <see cref="ISequenceDefinitionService"/>'i orkestre eder.
/// </summary>
public sealed class GetSequenceDefinitionListQueryHandler
    : IRequestHandler<GetSequenceDefinitionListQuery, BaseResponse<PaginatedResponse<SequenceDefinitionListResponse>>>
{
    private readonly ISequenceDefinitionService _service;

    public GetSequenceDefinitionListQueryHandler(ISequenceDefinitionService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<SequenceDefinitionListResponse>>> Handle(
        GetSequenceDefinitionListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
