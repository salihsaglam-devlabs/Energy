using Energy.Application.Requests.RequestType.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Requests.RequestType.Commands.DeleteRequestType;

/// <summary>
/// <see cref="DeleteRequestTypeCommand"/> handler'ı. <see cref="IRequestTypeService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteRequestTypeCommandHandler
    : IRequestHandler<DeleteRequestTypeCommand, BaseResponse<bool>>
{
    private readonly IRequestTypeService _service;

    public DeleteRequestTypeCommandHandler(IRequestTypeService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteRequestTypeCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
