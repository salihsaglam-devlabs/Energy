using Energy.Application.Documents.DocumentVersion.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Documents.DocumentVersion.Commands.CreateDocumentVersion;

/// <summary>
/// <see cref="CreateDocumentVersionCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IDocumentVersionService"/>'i orkestre eder.
/// </summary>
public sealed class CreateDocumentVersionCommandHandler
    : IRequestHandler<CreateDocumentVersionCommand, BaseResponse<Guid>>
{
    private readonly IDocumentVersionService _service;

    public CreateDocumentVersionCommandHandler(IDocumentVersionService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateDocumentVersionCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
