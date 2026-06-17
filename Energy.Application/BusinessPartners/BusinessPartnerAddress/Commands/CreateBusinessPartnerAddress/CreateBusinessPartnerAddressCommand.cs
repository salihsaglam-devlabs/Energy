using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerAddress.Requests;
using MediatR;

namespace Energy.Application.BusinessPartners.BusinessPartnerAddress.Commands.CreateBusinessPartnerAddress;

/// <summary>Yeni BusinessPartnerAddress oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateBusinessPartnerAddressCommand(CreateBusinessPartnerAddressRequest Request)
    : IRequest<BaseResponse<Guid>>;
