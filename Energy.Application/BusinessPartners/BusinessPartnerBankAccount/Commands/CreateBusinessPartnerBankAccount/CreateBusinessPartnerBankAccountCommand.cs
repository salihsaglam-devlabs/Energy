using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerBankAccount.Requests;
using MediatR;

namespace Energy.Application.BusinessPartners.BusinessPartnerBankAccount.Commands.CreateBusinessPartnerBankAccount;

/// <summary>Yeni BusinessPartnerBankAccount oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateBusinessPartnerBankAccountCommand(CreateBusinessPartnerBankAccountRequest Request)
    : IRequest<BaseResponse<Guid>>;
