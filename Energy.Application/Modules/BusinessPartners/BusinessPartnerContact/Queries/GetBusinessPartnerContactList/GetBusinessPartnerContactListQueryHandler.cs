using Energy.Application.Modules.BusinessPartners.BusinessPartnerContact.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerContact.Responses;
using MediatR;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartnerContact.Queries.GetBusinessPartnerContactList;

/// <summary>
/// <see cref="GetBusinessPartnerContactListQuery"/> handler'ı. <see cref="IBusinessPartnerContactService"/>'i orkestre eder.
/// </summary>
public sealed class GetBusinessPartnerContactListQueryHandler
    : IRequestHandler<GetBusinessPartnerContactListQuery, BaseResponse<PaginatedResponse<BusinessPartnerContactListResponse>>>
{
    private readonly IBusinessPartnerContactService _service;

    public GetBusinessPartnerContactListQueryHandler(IBusinessPartnerContactService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<BusinessPartnerContactListResponse>>> Handle(
        GetBusinessPartnerContactListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
