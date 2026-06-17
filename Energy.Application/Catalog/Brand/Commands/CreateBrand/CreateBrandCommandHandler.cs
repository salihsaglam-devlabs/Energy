using Energy.Application.Catalog.Brand.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Catalog.Brand.Commands.CreateBrand;

/// <summary>
/// <see cref="CreateBrandCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IBrandService"/>'i orkestre eder.
/// </summary>
public sealed class CreateBrandCommandHandler
    : IRequestHandler<CreateBrandCommand, BaseResponse<Guid>>
{
    private readonly IBrandService _service;

    public CreateBrandCommandHandler(IBrandService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateBrandCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
