using Energy.Application.Modules.Documents.DocumentVersion.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.DocumentVersion.Commands.UpdateDocumentVersion;

/// <summary>
/// <see cref="UpdateDocumentVersionCommand"/> handler'ı. <see cref="IDocumentVersionService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateDocumentVersionCommandHandler
    : IRequestHandler<UpdateDocumentVersionCommand, BaseResponse<bool>>
{
    private readonly IDocumentVersionService _service;

    public UpdateDocumentVersionCommandHandler(IDocumentVersionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateDocumentVersionCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
