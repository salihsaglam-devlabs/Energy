using Energy.Application.Modules.Requests.RequestLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Requests.RequestLine.Commands.DeleteRequestLine;

/// <summary>
/// <see cref="DeleteRequestLineCommand"/> handler'ı. <see cref="IRequestLineService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteRequestLineCommandHandler
    : IRequestHandler<DeleteRequestLineCommand, BaseResponse<bool>>
{
    private readonly IRequestLineService _service;

    public DeleteRequestLineCommandHandler(IRequestLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteRequestLineCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
