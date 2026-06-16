using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialAccount.Requests;
using MediatR;

namespace Energy.Application.Modules.Finance.FinancialAccount.Commands.CreateFinancialAccount;

/// <summary>Yeni FinancialAccount oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateFinancialAccountCommand(CreateFinancialAccountRequest Request)
    : IRequest<BaseResponse<Guid>>;
