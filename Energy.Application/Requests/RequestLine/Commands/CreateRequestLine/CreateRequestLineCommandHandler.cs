using Energy.Application.Requests.RequestLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Requests.RequestLine.Commands.CreateRequestLine;

/// <summary>
/// <see cref="CreateRequestLineCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IRequestLineService"/>'i orkestre eder.
/// </summary>
public sealed class CreateRequestLineCommandHandler
    : IRequestHandler<CreateRequestLineCommand, BaseResponse<Guid>>
{
    private readonly IRequestLineService _service;

    public CreateRequestLineCommandHandler(IRequestLineService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateRequestLineCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
