using Energy.Application.Modules.Documents.DocumentPermission.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentPermission.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.DocumentPermission.Queries.GetDocumentPermissionList;

/// <summary>
/// <see cref="GetDocumentPermissionListQuery"/> handler'ı. <see cref="IDocumentPermissionService"/>'i orkestre eder.
/// </summary>
public sealed class GetDocumentPermissionListQueryHandler
    : IRequestHandler<GetDocumentPermissionListQuery, BaseResponse<PaginatedResponse<DocumentPermissionListResponse>>>
{
    private readonly IDocumentPermissionService _service;

    public GetDocumentPermissionListQueryHandler(IDocumentPermissionService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<DocumentPermissionListResponse>>> Handle(
        GetDocumentPermissionListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
