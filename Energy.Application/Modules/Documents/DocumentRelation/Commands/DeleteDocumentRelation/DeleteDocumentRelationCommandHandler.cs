using Energy.Application.Modules.Documents.DocumentRelation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.DocumentRelation.Commands.DeleteDocumentRelation;

/// <summary>
/// <see cref="DeleteDocumentRelationCommand"/> handler'ı. <see cref="IDocumentRelationService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteDocumentRelationCommandHandler
    : IRequestHandler<DeleteDocumentRelationCommand, BaseResponse<bool>>
{
    private readonly IDocumentRelationService _service;

    public DeleteDocumentRelationCommandHandler(IDocumentRelationService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteDocumentRelationCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
