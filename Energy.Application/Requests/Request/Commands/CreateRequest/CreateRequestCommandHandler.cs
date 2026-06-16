using Energy.Application.Requests.Request.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Requests.Request.Commands.CreateRequest;

/// <summary>
/// <see cref="CreateRequestCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IRequestService"/>'i orkestre eder.
/// </summary>
public sealed class CreateRequestCommandHandler
    : IRequestHandler<CreateRequestCommand, BaseResponse<Guid>>
{
    private readonly IRequestService _service;

    public CreateRequestCommandHandler(IRequestService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateRequestCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
