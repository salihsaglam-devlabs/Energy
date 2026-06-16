using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartner.Requests;
using MediatR;

namespace Energy.Application.BusinessPartners.BusinessPartner.Commands.UpdateBusinessPartner;

/// <summary>Var olan BusinessPartner kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateBusinessPartnerCommand(Guid Id, UpdateBusinessPartnerRequest Request)
    : IRequest<BaseResponse<bool>>;
