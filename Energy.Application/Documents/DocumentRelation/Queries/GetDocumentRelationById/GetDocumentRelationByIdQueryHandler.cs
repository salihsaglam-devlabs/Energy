using Energy.Application.Documents.DocumentRelation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentRelation.Responses;
using MediatR;

namespace Energy.Application.Documents.DocumentRelation.Queries.GetDocumentRelationById;

/// <summary>
/// <see cref="GetDocumentRelationByIdQuery"/> handler'ı. <see cref="IDocumentRelationService"/>'i orkestre eder.
/// </summary>
public sealed class GetDocumentRelationByIdQueryHandler
    : IRequestHandler<GetDocumentRelationByIdQuery, BaseResponse<DocumentRelationDetailResponse>>
{
    private readonly IDocumentRelationService _service;

    public GetDocumentRelationByIdQueryHandler(IDocumentRelationService service)
        => _service = service;

    public Task<BaseResponse<DocumentRelationDetailResponse>> Handle(
        GetDocumentRelationByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
