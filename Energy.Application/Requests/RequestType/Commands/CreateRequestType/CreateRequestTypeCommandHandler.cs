using Energy.Application.Requests.RequestType.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Requests.RequestType.Commands.CreateRequestType;

/// <summary>
/// <see cref="CreateRequestTypeCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IRequestTypeService"/>'i orkestre eder.
/// </summary>
public sealed class CreateRequestTypeCommandHandler
    : IRequestHandler<CreateRequestTypeCommand, BaseResponse<Guid>>
{
    private readonly IRequestTypeService _service;

    public CreateRequestTypeCommandHandler(IRequestTypeService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateRequestTypeCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
