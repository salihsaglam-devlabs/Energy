using Energy.Application.Documents.DocumentRelation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Documents.DocumentRelation.Commands.UpdateDocumentRelation;

/// <summary>
/// <see cref="UpdateDocumentRelationCommand"/> handler'ı. <see cref="IDocumentRelationService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateDocumentRelationCommandHandler
    : IRequestHandler<UpdateDocumentRelationCommand, BaseResponse<bool>>
{
    private readonly IDocumentRelationService _service;

    public UpdateDocumentRelationCommandHandler(IDocumentRelationService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateDocumentRelationCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
