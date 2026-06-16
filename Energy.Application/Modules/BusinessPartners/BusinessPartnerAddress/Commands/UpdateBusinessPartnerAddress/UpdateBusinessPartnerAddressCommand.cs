using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerAddress.Requests;
using MediatR;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartnerAddress.Commands.UpdateBusinessPartnerAddress;

/// <summary>Var olan BusinessPartnerAddress kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateBusinessPartnerAddressCommand(Guid Id, UpdateBusinessPartnerAddressRequest Request)
    : IRequest<BaseResponse<bool>>;
