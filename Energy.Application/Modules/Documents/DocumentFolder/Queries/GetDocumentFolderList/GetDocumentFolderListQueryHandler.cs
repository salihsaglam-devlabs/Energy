using Energy.Application.Modules.Documents.DocumentFolder.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentFolder.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.DocumentFolder.Queries.GetDocumentFolderList;

/// <summary>
/// <see cref="GetDocumentFolderListQuery"/> handler'ı. <see cref="IDocumentFolderService"/>'i orkestre eder.
/// </summary>
public sealed class GetDocumentFolderListQueryHandler
    : IRequestHandler<GetDocumentFolderListQuery, BaseResponse<PaginatedResponse<DocumentFolderListResponse>>>
{
    private readonly IDocumentFolderService _service;

    public GetDocumentFolderListQueryHandler(IDocumentFolderService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<DocumentFolderListResponse>>> Handle(
        GetDocumentFolderListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
