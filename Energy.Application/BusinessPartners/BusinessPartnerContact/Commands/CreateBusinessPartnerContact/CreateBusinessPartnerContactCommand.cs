using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerContact.Requests;
using MediatR;

namespace Energy.Application.BusinessPartners.BusinessPartnerContact.Commands.CreateBusinessPartnerContact;

/// <summary>Yeni BusinessPartnerContact oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateBusinessPartnerContactCommand(CreateBusinessPartnerContactRequest Request)
    : IRequest<BaseResponse<Guid>>;
