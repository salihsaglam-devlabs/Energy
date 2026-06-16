using Energy.Application.Modules.Documents.Document.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.Document.Commands.DeleteDocument;

/// <summary>
/// <see cref="DeleteDocumentCommand"/> handler'ı. <see cref="IDocumentService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteDocumentCommandHandler
    : IRequestHandler<DeleteDocumentCommand, BaseResponse<bool>>
{
    private readonly IDocumentService _service;

    public DeleteDocumentCommandHandler(IDocumentService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteDocumentCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
