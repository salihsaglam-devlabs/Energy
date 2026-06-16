using Energy.Application.Finance.Collection.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Finance.Collection.Commands.UpdateCollection;

/// <summary>
/// <see cref="UpdateCollectionCommand"/> handler'ı. <see cref="ICollectionService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateCollectionCommandHandler
    : IRequestHandler<UpdateCollectionCommand, BaseResponse<bool>>
{
    private readonly ICollectionService _service;

    public UpdateCollectionCommandHandler(ICollectionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateCollectionCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
