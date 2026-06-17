using Energy.Application.BusinessPartners.BusinessPartner.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartner.Responses;
using MediatR;

namespace Energy.Application.BusinessPartners.BusinessPartner.Queries.GetBusinessPartnerList;

/// <summary>
/// <see cref="GetBusinessPartnerListQuery"/> handler'ı. <see cref="IBusinessPartnerService"/>'i orkestre eder.
/// </summary>
public sealed class GetBusinessPartnerListQueryHandler
    : IRequestHandler<GetBusinessPartnerListQuery, BaseResponse<PaginatedResponse<BusinessPartnerListResponse>>>
{
    private readonly IBusinessPartnerService _service;

    public GetBusinessPartnerListQueryHandler(IBusinessPartnerService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<BusinessPartnerListResponse>>> Handle(
        GetBusinessPartnerListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
