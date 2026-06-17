using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.BusinessPartners.BusinessPartnerBankAccount.Commands.DeleteBusinessPartnerBankAccount;

/// <summary>BusinessPartnerBankAccount kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteBusinessPartnerBankAccountCommand(Guid Id) : IRequest<BaseResponse<bool>>;
