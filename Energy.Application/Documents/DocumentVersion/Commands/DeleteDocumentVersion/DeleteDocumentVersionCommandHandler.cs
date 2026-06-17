using Energy.Application.Documents.DocumentVersion.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Documents.DocumentVersion.Commands.DeleteDocumentVersion;

/// <summary>
/// <see cref="DeleteDocumentVersionCommand"/> handler'ı. <see cref="IDocumentVersionService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteDocumentVersionCommandHandler
    : IRequestHandler<DeleteDocumentVersionCommand, BaseResponse<bool>>
{
    private readonly IDocumentVersionService _service;

    public DeleteDocumentVersionCommandHandler(IDocumentVersionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteDocumentVersionCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
