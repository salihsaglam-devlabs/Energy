using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerContact.Requests;
using MediatR;

namespace Energy.Application.BusinessPartners.BusinessPartnerContact.Commands.UpdateBusinessPartnerContact;

/// <summary>Var olan BusinessPartnerContact kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateBusinessPartnerContactCommand(Guid Id, UpdateBusinessPartnerContactRequest Request)
    : IRequest<BaseResponse<bool>>;
