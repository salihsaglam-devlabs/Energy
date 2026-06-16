using Energy.Application.Catalog.Brand.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Catalog.Brand.Commands.UpdateBrand;

/// <summary>
/// <see cref="UpdateBrandCommand"/> handler'ı. <see cref="IBrandService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateBrandCommandHandler
    : IRequestHandler<UpdateBrandCommand, BaseResponse<bool>>
{
    private readonly IBrandService _service;

    public UpdateBrandCommandHandler(IBrandService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateBrandCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
