using Energy.Application.Organization.LeaveRequest.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Organization.LeaveRequest.Commands.CreateLeaveRequest;

/// <summary>
/// <see cref="CreateLeaveRequestCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="ILeaveRequestService"/>'i orkestre eder.
/// </summary>
public sealed class CreateLeaveRequestCommandHandler
    : IRequestHandler<CreateLeaveRequestCommand, BaseResponse<Guid>>
{
    private readonly ILeaveRequestService _service;

    public CreateLeaveRequestCommandHandler(ILeaveRequestService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateLeaveRequestCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
