using Energy.Application.Modules.Core.Company.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Company.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.Company.Queries.GetCompanyById;

/// <summary>
/// <see cref="GetCompanyByIdQuery"/> handler'ı. <see cref="ICompanyService"/>'i orkestre eder.
/// </summary>
public sealed class GetCompanyByIdQueryHandler
    : IRequestHandler<GetCompanyByIdQuery, BaseResponse<CompanyDetailResponse>>
{
    private readonly ICompanyService _service;

    public GetCompanyByIdQueryHandler(ICompanyService service)
        => _service = service;

    public Task<BaseResponse<CompanyDetailResponse>> Handle(
        GetCompanyByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
