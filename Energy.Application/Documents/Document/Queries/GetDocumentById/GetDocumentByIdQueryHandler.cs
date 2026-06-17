using Energy.Application.Documents.Document.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.Document.Responses;
using MediatR;

namespace Energy.Application.Documents.Document.Queries.GetDocumentById;

/// <summary>
/// <see cref="GetDocumentByIdQuery"/> handler'ı. <see cref="IDocumentService"/>'i orkestre eder.
/// </summary>
public sealed class GetDocumentByIdQueryHandler
    : IRequestHandler<GetDocumentByIdQuery, BaseResponse<DocumentDetailResponse>>
{
    private readonly IDocumentService _service;

    public GetDocumentByIdQueryHandler(IDocumentService service)
        => _service = service;

    public Task<BaseResponse<DocumentDetailResponse>> Handle(
        GetDocumentByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
