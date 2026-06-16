using Energy.Application.Modules.BusinessPartners.BusinessPartnerBankAccount.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerBankAccount.Responses;
using MediatR;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartnerBankAccount.Queries.GetBusinessPartnerBankAccountList;

/// <summary>
/// <see cref="GetBusinessPartnerBankAccountListQuery"/> handler'ı. <see cref="IBusinessPartnerBankAccountService"/>'i orkestre eder.
/// </summary>
public sealed class GetBusinessPartnerBankAccountListQueryHandler
    : IRequestHandler<GetBusinessPartnerBankAccountListQuery, BaseResponse<PaginatedResponse<BusinessPartnerBankAccountListResponse>>>
{
    private readonly IBusinessPartnerBankAccountService _service;

    public GetBusinessPartnerBankAccountListQueryHandler(IBusinessPartnerBankAccountService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<BusinessPartnerBankAccountListResponse>>> Handle(
        GetBusinessPartnerBankAccountListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
