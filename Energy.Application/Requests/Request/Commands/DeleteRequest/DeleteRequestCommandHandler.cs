using Energy.Application.Requests.Request.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Requests.Request.Commands.DeleteRequest;

/// <summary>
/// <see cref="DeleteRequestCommand"/> handler'ı. <see cref="IRequestService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteRequestCommandHandler
    : IRequestHandler<DeleteRequestCommand, BaseResponse<bool>>
{
    private readonly IRequestService _service;

    public DeleteRequestCommandHandler(IRequestService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteRequestCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
