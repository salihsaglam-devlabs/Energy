using Energy.Application.Modules.Documents.DocumentPermission.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.DocumentPermission.Commands.UpdateDocumentPermission;

/// <summary>
/// <see cref="UpdateDocumentPermissionCommand"/> handler'ı. <see cref="IDocumentPermissionService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateDocumentPermissionCommandHandler
    : IRequestHandler<UpdateDocumentPermissionCommand, BaseResponse<bool>>
{
    private readonly IDocumentPermissionService _service;

    public UpdateDocumentPermissionCommandHandler(IDocumentPermissionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateDocumentPermissionCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
