using Energy.Application.Documents.DocumentFolder.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Documents.DocumentFolder.Commands.UpdateDocumentFolder;

/// <summary>
/// <see cref="UpdateDocumentFolderCommand"/> handler'ı. <see cref="IDocumentFolderService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateDocumentFolderCommandHandler
    : IRequestHandler<UpdateDocumentFolderCommand, BaseResponse<bool>>
{
    private readonly IDocumentFolderService _service;

    public UpdateDocumentFolderCommandHandler(IDocumentFolderService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateDocumentFolderCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
