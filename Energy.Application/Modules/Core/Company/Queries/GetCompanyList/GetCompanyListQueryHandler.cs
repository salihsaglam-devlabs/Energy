using Energy.Application.Modules.Core.Company.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Company.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.Company.Queries.GetCompanyList;

/// <summary>
/// <see cref="GetCompanyListQuery"/> handler'ı. <see cref="ICompanyService"/>'i orkestre eder.
/// </summary>
public sealed class GetCompanyListQueryHandler
    : IRequestHandler<GetCompanyListQuery, BaseResponse<PaginatedResponse<CompanyListResponse>>>
{
    private readonly ICompanyService _service;

    public GetCompanyListQueryHandler(ICompanyService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<CompanyListResponse>>> Handle(
        GetCompanyListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
