using Energy.Application.Modules.Core.SequenceDefinition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.SequenceDefinition.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.SequenceDefinition.Queries.GetSequenceDefinitionById;

/// <summary>
/// <see cref="GetSequenceDefinitionByIdQuery"/> handler'ı. <see cref="ISequenceDefinitionService"/>'i orkestre eder.
/// </summary>
public sealed class GetSequenceDefinitionByIdQueryHandler
    : IRequestHandler<GetSequenceDefinitionByIdQuery, BaseResponse<SequenceDefinitionDetailResponse>>
{
    private readonly ISequenceDefinitionService _service;

    public GetSequenceDefinitionByIdQueryHandler(ISequenceDefinitionService service)
        => _service = service;

    public Task<BaseResponse<SequenceDefinitionDetailResponse>> Handle(
        GetSequenceDefinitionByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
