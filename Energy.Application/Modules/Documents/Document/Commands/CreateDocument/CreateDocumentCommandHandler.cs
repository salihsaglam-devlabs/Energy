using Energy.Application.Modules.Documents.Document.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.Document.Commands.CreateDocument;

/// <summary>
/// <see cref="CreateDocumentCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IDocumentService"/>'i orkestre eder.
/// </summary>
public sealed class CreateDocumentCommandHandler
    : IRequestHandler<CreateDocumentCommand, BaseResponse<Guid>>
{
    private readonly IDocumentService _service;

    public CreateDocumentCommandHandler(IDocumentService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateDocumentCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
