using Energy.Application.Organization.LeaveRequest.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Organization.LeaveRequest.Commands.DeleteLeaveRequest;

/// <summary>
/// <see cref="DeleteLeaveRequestCommand"/> handler'ı. <see cref="ILeaveRequestService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteLeaveRequestCommandHandler
    : IRequestHandler<DeleteLeaveRequestCommand, BaseResponse<bool>>
{
    private readonly ILeaveRequestService _service;

    public DeleteLeaveRequestCommandHandler(ILeaveRequestService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteLeaveRequestCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
