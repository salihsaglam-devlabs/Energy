using Energy.Application.Documents.DocumentVersion.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentVersion.Responses;
using MediatR;

namespace Energy.Application.Documents.DocumentVersion.Queries.GetDocumentVersionList;

/// <summary>
/// <see cref="GetDocumentVersionListQuery"/> handler'ı. <see cref="IDocumentVersionService"/>'i orkestre eder.
/// </summary>
public sealed class GetDocumentVersionListQueryHandler
    : IRequestHandler<GetDocumentVersionListQuery, BaseResponse<PaginatedResponse<DocumentVersionListResponse>>>
{
    private readonly IDocumentVersionService _service;

    public GetDocumentVersionListQueryHandler(IDocumentVersionService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<DocumentVersionListResponse>>> Handle(
        GetDocumentVersionListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
