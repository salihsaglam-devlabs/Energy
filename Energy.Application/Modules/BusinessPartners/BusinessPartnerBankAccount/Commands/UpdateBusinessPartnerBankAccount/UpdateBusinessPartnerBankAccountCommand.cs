using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerBankAccount.Requests;
using MediatR;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartnerBankAccount.Commands.UpdateBusinessPartnerBankAccount;

/// <summary>Var olan BusinessPartnerBankAccount kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateBusinessPartnerBankAccountCommand(Guid Id, UpdateBusinessPartnerBankAccountRequest Request)
    : IRequest<BaseResponse<bool>>;
