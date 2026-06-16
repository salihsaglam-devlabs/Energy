using Energy.Application.Modules.Documents.DocumentRelation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.DocumentRelation.Commands.CreateDocumentRelation;

/// <summary>
/// <see cref="CreateDocumentRelationCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IDocumentRelationService"/>'i orkestre eder.
/// </summary>
public sealed class CreateDocumentRelationCommandHandler
    : IRequestHandler<CreateDocumentRelationCommand, BaseResponse<Guid>>
{
    private readonly IDocumentRelationService _service;

    public CreateDocumentRelationCommandHandler(IDocumentRelationService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateDocumentRelationCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
