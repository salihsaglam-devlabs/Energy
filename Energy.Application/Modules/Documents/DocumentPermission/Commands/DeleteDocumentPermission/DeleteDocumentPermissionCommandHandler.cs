using Energy.Application.Modules.Documents.DocumentPermission.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.DocumentPermission.Commands.DeleteDocumentPermission;

/// <summary>
/// <see cref="DeleteDocumentPermissionCommand"/> handler'ı. <see cref="IDocumentPermissionService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteDocumentPermissionCommandHandler
    : IRequestHandler<DeleteDocumentPermissionCommand, BaseResponse<bool>>
{
    private readonly IDocumentPermissionService _service;

    public DeleteDocumentPermissionCommandHandler(IDocumentPermissionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteDocumentPermissionCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
