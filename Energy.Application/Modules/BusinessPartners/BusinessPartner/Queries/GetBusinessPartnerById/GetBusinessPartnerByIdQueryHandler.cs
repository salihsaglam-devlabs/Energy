using Energy.Application.Modules.BusinessPartners.BusinessPartner.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartner.Responses;
using MediatR;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartner.Queries.GetBusinessPartnerById;

/// <summary>
/// <see cref="GetBusinessPartnerByIdQuery"/> handler'ı. <see cref="IBusinessPartnerService"/>'i orkestre eder.
/// </summary>
public sealed class GetBusinessPartnerByIdQueryHandler
    : IRequestHandler<GetBusinessPartnerByIdQuery, BaseResponse<BusinessPartnerDetailResponse>>
{
    private readonly IBusinessPartnerService _service;

    public GetBusinessPartnerByIdQueryHandler(IBusinessPartnerService service)
        => _service = service;

    public Task<BaseResponse<BusinessPartnerDetailResponse>> Handle(
        GetBusinessPartnerByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
