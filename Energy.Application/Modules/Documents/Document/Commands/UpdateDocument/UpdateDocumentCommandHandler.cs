using Energy.Application.Modules.Documents.Document.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.Document.Commands.UpdateDocument;

/// <summary>
/// <see cref="UpdateDocumentCommand"/> handler'ı. <see cref="IDocumentService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateDocumentCommandHandler
    : IRequestHandler<UpdateDocumentCommand, BaseResponse<bool>>
{
    private readonly IDocumentService _service;

    public UpdateDocumentCommandHandler(IDocumentService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateDocumentCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
