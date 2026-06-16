using Energy.Application.Modules.Finance.CollectionAllocation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.CollectionAllocation.Commands.CreateCollectionAllocation;

/// <summary>
/// <see cref="CreateCollectionAllocationCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="ICollectionAllocationService"/>'i orkestre eder.
/// </summary>
public sealed class CreateCollectionAllocationCommandHandler
    : IRequestHandler<CreateCollectionAllocationCommand, BaseResponse<Guid>>
{
    private readonly ICollectionAllocationService _service;

    public CreateCollectionAllocationCommandHandler(ICollectionAllocationService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateCollectionAllocationCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
