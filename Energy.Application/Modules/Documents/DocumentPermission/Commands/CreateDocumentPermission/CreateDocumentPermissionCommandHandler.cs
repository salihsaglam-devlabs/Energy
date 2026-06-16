using Energy.Application.Modules.Documents.DocumentPermission.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.DocumentPermission.Commands.CreateDocumentPermission;

/// <summary>
/// <see cref="CreateDocumentPermissionCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IDocumentPermissionService"/>'i orkestre eder.
/// </summary>
public sealed class CreateDocumentPermissionCommandHandler
    : IRequestHandler<CreateDocumentPermissionCommand, BaseResponse<Guid>>
{
    private readonly IDocumentPermissionService _service;

    public CreateDocumentPermissionCommandHandler(IDocumentPermissionService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateDocumentPermissionCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
