using Energy.Application.Organization.LeaveRequest.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Organization.LeaveRequest.Commands.UpdateLeaveRequest;

/// <summary>
/// <see cref="UpdateLeaveRequestCommand"/> handler'ı. <see cref="ILeaveRequestService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateLeaveRequestCommandHandler
    : IRequestHandler<UpdateLeaveRequestCommand, BaseResponse<bool>>
{
    private readonly ILeaveRequestService _service;

    public UpdateLeaveRequestCommandHandler(ILeaveRequestService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateLeaveRequestCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
