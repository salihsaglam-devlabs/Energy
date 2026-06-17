using Energy.Application.Documents.DocumentRelation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentRelation.Responses;
using MediatR;

namespace Energy.Application.Documents.DocumentRelation.Queries.GetDocumentRelationList;

/// <summary>
/// <see cref="GetDocumentRelationListQuery"/> handler'ı. <see cref="IDocumentRelationService"/>'i orkestre eder.
/// </summary>
public sealed class GetDocumentRelationListQueryHandler
    : IRequestHandler<GetDocumentRelationListQuery, BaseResponse<PaginatedResponse<DocumentRelationListResponse>>>
{
    private readonly IDocumentRelationService _service;

    public GetDocumentRelationListQueryHandler(IDocumentRelationService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<DocumentRelationListResponse>>> Handle(
        GetDocumentRelationListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
