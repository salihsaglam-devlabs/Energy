using Energy.Application.Finance.Collection.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Finance.Collection.Commands.CreateCollection;

/// <summary>
/// <see cref="CreateCollectionCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="ICollectionService"/>'i orkestre eder.
/// </summary>
public sealed class CreateCollectionCommandHandler
    : IRequestHandler<CreateCollectionCommand, BaseResponse<Guid>>
{
    private readonly ICollectionService _service;

    public CreateCollectionCommandHandler(ICollectionService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateCollectionCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
