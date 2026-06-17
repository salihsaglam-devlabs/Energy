using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.ExpenseClaim.Requests;
using MediatR;

namespace Energy.Application.Organization.ExpenseClaim.Commands.CreateExpenseClaim;

/// <summary>Yeni ExpenseClaim oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateExpenseClaimCommand(CreateExpenseClaimRequest Request)
    : IRequest<BaseResponse<Guid>>;
