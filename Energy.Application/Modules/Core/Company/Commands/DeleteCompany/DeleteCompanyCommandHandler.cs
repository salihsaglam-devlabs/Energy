using Energy.Application.Modules.Core.Company.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.Company.Commands.DeleteCompany;

/// <summary>
/// <see cref="DeleteCompanyCommand"/> handler'ı. <see cref="ICompanyService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteCompanyCommandHandler
    : IRequestHandler<DeleteCompanyCommand, BaseResponse<bool>>
{
    private readonly ICompanyService _service;

    public DeleteCompanyCommandHandler(ICompanyService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteCompanyCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
