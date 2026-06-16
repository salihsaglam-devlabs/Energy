using Energy.Application.Modules.Requests.Request.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Requests.Request.Commands.UpdateRequest;

/// <summary>
/// <see cref="UpdateRequestCommand"/> handler'ı. <see cref="IRequestService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateRequestCommandHandler
    : IRequestHandler<UpdateRequestCommand, BaseResponse<bool>>
{
    private readonly IRequestService _service;

    public UpdateRequestCommandHandler(IRequestService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateRequestCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
