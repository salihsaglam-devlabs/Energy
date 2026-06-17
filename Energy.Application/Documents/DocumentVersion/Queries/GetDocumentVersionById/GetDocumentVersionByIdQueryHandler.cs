using Energy.Application.Documents.DocumentVersion.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentVersion.Responses;
using MediatR;

namespace Energy.Application.Documents.DocumentVersion.Queries.GetDocumentVersionById;

/// <summary>
/// <see cref="GetDocumentVersionByIdQuery"/> handler'ı. <see cref="IDocumentVersionService"/>'i orkestre eder.
/// </summary>
public sealed class GetDocumentVersionByIdQueryHandler
    : IRequestHandler<GetDocumentVersionByIdQuery, BaseResponse<DocumentVersionDetailResponse>>
{
    private readonly IDocumentVersionService _service;

    public GetDocumentVersionByIdQueryHandler(IDocumentVersionService service)
        => _service = service;

    public Task<BaseResponse<DocumentVersionDetailResponse>> Handle(
        GetDocumentVersionByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
