using Energy.Application.Documents.DocumentFolder.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Documents.DocumentFolder.Commands.DeleteDocumentFolder;

/// <summary>
/// <see cref="DeleteDocumentFolderCommand"/> handler'ı. <see cref="IDocumentFolderService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteDocumentFolderCommandHandler
    : IRequestHandler<DeleteDocumentFolderCommand, BaseResponse<bool>>
{
    private readonly IDocumentFolderService _service;

    public DeleteDocumentFolderCommandHandler(IDocumentFolderService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteDocumentFolderCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
