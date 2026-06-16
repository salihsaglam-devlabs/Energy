using Energy.Application.Modules.Documents.DocumentFolder.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.DocumentFolder.Commands.CreateDocumentFolder;

/// <summary>
/// <see cref="CreateDocumentFolderCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IDocumentFolderService"/>'i orkestre eder.
/// </summary>
public sealed class CreateDocumentFolderCommandHandler
    : IRequestHandler<CreateDocumentFolderCommand, BaseResponse<Guid>>
{
    private readonly IDocumentFolderService _service;

    public CreateDocumentFolderCommandHandler(IDocumentFolderService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateDocumentFolderCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
