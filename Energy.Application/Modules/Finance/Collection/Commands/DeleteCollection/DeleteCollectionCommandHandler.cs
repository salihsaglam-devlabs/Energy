using Energy.Application.Modules.Finance.Collection.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.Collection.Commands.DeleteCollection;

/// <summary>
/// <see cref="DeleteCollectionCommand"/> handler'ı. <see cref="ICollectionService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteCollectionCommandHandler
    : IRequestHandler<DeleteCollectionCommand, BaseResponse<bool>>
{
    private readonly ICollectionService _service;

    public DeleteCollectionCommandHandler(ICollectionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteCollectionCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
