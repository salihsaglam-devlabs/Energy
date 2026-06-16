using Energy.Application.Modules.Catalog.MaterialCategory.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialCategory.Commands.CreateMaterialCategory;

/// <summary>
/// <see cref="CreateMaterialCategoryCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IMaterialCategoryService"/>'i orkestre eder.
/// </summary>
public sealed class CreateMaterialCategoryCommandHandler
    : IRequestHandler<CreateMaterialCategoryCommand, BaseResponse<Guid>>
{
    private readonly IMaterialCategoryService _service;

    public CreateMaterialCategoryCommandHandler(IMaterialCategoryService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateMaterialCategoryCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
