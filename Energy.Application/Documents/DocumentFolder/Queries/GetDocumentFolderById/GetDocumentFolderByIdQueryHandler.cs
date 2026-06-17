using Energy.Application.Documents.DocumentFolder.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentFolder.Responses;
using MediatR;

namespace Energy.Application.Documents.DocumentFolder.Queries.GetDocumentFolderById;

/// <summary>
/// <see cref="GetDocumentFolderByIdQuery"/> handler'ı. <see cref="IDocumentFolderService"/>'i orkestre eder.
/// </summary>
public sealed class GetDocumentFolderByIdQueryHandler
    : IRequestHandler<GetDocumentFolderByIdQuery, BaseResponse<DocumentFolderDetailResponse>>
{
    private readonly IDocumentFolderService _service;

    public GetDocumentFolderByIdQueryHandler(IDocumentFolderService service)
        => _service = service;

    public Task<BaseResponse<DocumentFolderDetailResponse>> Handle(
        GetDocumentFolderByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
