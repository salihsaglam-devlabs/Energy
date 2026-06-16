using Energy.Application.Modules.Requests.RequestType.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Requests.RequestType.Commands.UpdateRequestType;

/// <summary>
/// <see cref="UpdateRequestTypeCommand"/> handler'ı. <see cref="IRequestTypeService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateRequestTypeCommandHandler
    : IRequestHandler<UpdateRequestTypeCommand, BaseResponse<bool>>
{
    private readonly IRequestTypeService _service;

    public UpdateRequestTypeCommandHandler(IRequestTypeService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateRequestTypeCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
