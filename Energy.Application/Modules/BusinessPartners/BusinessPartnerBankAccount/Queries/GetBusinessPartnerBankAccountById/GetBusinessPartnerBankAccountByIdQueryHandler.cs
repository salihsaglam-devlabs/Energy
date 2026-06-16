using Energy.Application.Modules.BusinessPartners.BusinessPartnerBankAccount.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerBankAccount.Responses;
using MediatR;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartnerBankAccount.Queries.GetBusinessPartnerBankAccountById;

/// <summary>
/// <see cref="GetBusinessPartnerBankAccountByIdQuery"/> handler'ı. <see cref="IBusinessPartnerBankAccountService"/>'i orkestre eder.
/// </summary>
public sealed class GetBusinessPartnerBankAccountByIdQueryHandler
    : IRequestHandler<GetBusinessPartnerBankAccountByIdQuery, BaseResponse<BusinessPartnerBankAccountDetailResponse>>
{
    private readonly IBusinessPartnerBankAccountService _service;

    public GetBusinessPartnerBankAccountByIdQueryHandler(IBusinessPartnerBankAccountService service)
        => _service = service;

    public Task<BaseResponse<BusinessPartnerBankAccountDetailResponse>> Handle(
        GetBusinessPartnerBankAccountByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
