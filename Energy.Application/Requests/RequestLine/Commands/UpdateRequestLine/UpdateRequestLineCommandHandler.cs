using Energy.Application.Requests.RequestLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Requests.RequestLine.Commands.UpdateRequestLine;

/// <summary>
/// <see cref="UpdateRequestLineCommand"/> handler'ı. <see cref="IRequestLineService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateRequestLineCommandHandler
    : IRequestHandler<UpdateRequestLineCommand, BaseResponse<bool>>
{
    private readonly IRequestLineService _service;

    public UpdateRequestLineCommandHandler(IRequestLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateRequestLineCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
