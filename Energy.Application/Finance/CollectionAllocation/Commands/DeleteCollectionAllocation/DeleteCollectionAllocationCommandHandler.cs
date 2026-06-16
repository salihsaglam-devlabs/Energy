using Energy.Application.Finance.CollectionAllocation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Finance.CollectionAllocation.Commands.DeleteCollectionAllocation;

/// <summary>
/// <see cref="DeleteCollectionAllocationCommand"/> handler'ı. <see cref="ICollectionAllocationService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteCollectionAllocationCommandHandler
    : IRequestHandler<DeleteCollectionAllocationCommand, BaseResponse<bool>>
{
    private readonly ICollectionAllocationService _service;

    public DeleteCollectionAllocationCommandHandler(ICollectionAllocationService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteCollectionAllocationCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
