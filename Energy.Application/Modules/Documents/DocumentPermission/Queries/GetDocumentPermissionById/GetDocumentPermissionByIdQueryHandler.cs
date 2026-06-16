using Energy.Application.Modules.Documents.DocumentPermission.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentPermission.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.DocumentPermission.Queries.GetDocumentPermissionById;

/// <summary>
/// <see cref="GetDocumentPermissionByIdQuery"/> handler'ı. <see cref="IDocumentPermissionService"/>'i orkestre eder.
/// </summary>
public sealed class GetDocumentPermissionByIdQueryHandler
    : IRequestHandler<GetDocumentPermissionByIdQuery, BaseResponse<DocumentPermissionDetailResponse>>
{
    private readonly IDocumentPermissionService _service;

    public GetDocumentPermissionByIdQueryHandler(IDocumentPermissionService service)
        => _service = service;

    public Task<BaseResponse<DocumentPermissionDetailResponse>> Handle(
        GetDocumentPermissionByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
