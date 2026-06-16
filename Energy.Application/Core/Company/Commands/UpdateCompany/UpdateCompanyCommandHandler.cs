using Energy.Application.Core.Company.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Core.Company.Commands.UpdateCompany;

/// <summary>
/// <see cref="UpdateCompanyCommand"/> handler'ı. <see cref="ICompanyService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateCompanyCommandHandler
    : IRequestHandler<UpdateCompanyCommand, BaseResponse<bool>>
{
    private readonly ICompanyService _service;

    public UpdateCompanyCommandHandler(ICompanyService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateCompanyCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
