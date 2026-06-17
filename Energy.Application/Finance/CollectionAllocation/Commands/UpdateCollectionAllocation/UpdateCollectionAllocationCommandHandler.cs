using Energy.Application.Finance.CollectionAllocation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Finance.CollectionAllocation.Commands.UpdateCollectionAllocation;

/// <summary>
/// <see cref="UpdateCollectionAllocationCommand"/> handler'ı. <see cref="ICollectionAllocationService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateCollectionAllocationCommandHandler
    : IRequestHandler<UpdateCollectionAllocationCommand, BaseResponse<bool>>
{
    private readonly ICollectionAllocationService _service;

    public UpdateCollectionAllocationCommandHandler(ICollectionAllocationService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateCollectionAllocationCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
