using Energy.Application.BusinessPartners.BusinessPartnerContact.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerContact.Responses;
using MediatR;

namespace Energy.Application.BusinessPartners.BusinessPartnerContact.Queries.GetBusinessPartnerContactById;

/// <summary>
/// <see cref="GetBusinessPartnerContactByIdQuery"/> handler'ı. <see cref="IBusinessPartnerContactService"/>'i orkestre eder.
/// </summary>
public sealed class GetBusinessPartnerContactByIdQueryHandler
    : IRequestHandler<GetBusinessPartnerContactByIdQuery, BaseResponse<BusinessPartnerContactDetailResponse>>
{
    private readonly IBusinessPartnerContactService _service;

    public GetBusinessPartnerContactByIdQueryHandler(IBusinessPartnerContactService service)
        => _service = service;

    public Task<BaseResponse<BusinessPartnerContactDetailResponse>> Handle(
        GetBusinessPartnerContactByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
