using Energy.Application.Modules.Core.Company.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.Company.Commands.CreateCompany;

/// <summary>
/// <see cref="CreateCompanyCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="ICompanyService"/>'i orkestre eder.
/// </summary>
public sealed class CreateCompanyCommandHandler
    : IRequestHandler<CreateCompanyCommand, BaseResponse<Guid>>
{
    private readonly ICompanyService _service;

    public CreateCompanyCommandHandler(ICompanyService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateCompanyCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
