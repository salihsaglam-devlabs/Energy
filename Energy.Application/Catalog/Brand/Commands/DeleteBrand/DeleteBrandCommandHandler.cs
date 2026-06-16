using Energy.Application.Catalog.Brand.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Catalog.Brand.Commands.DeleteBrand;

/// <summary>
/// <see cref="DeleteBrandCommand"/> handler'ı. <see cref="IBrandService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteBrandCommandHandler
    : IRequestHandler<DeleteBrandCommand, BaseResponse<bool>>
{
    private readonly IBrandService _service;

    public DeleteBrandCommandHandler(IBrandService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteBrandCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
