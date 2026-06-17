using Energy.Application.Documents.Document.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.Document.Responses;
using MediatR;

namespace Energy.Application.Documents.Document.Queries.GetDocumentList;

/// <summary>
/// <see cref="GetDocumentListQuery"/> handler'ı. <see cref="IDocumentService"/>'i orkestre eder.
/// </summary>
public sealed class GetDocumentListQueryHandler
    : IRequestHandler<GetDocumentListQuery, BaseResponse<PaginatedResponse<DocumentListResponse>>>
{
    private readonly IDocumentService _service;

    public GetDocumentListQueryHandler(IDocumentService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<DocumentListResponse>>> Handle(
        GetDocumentListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
