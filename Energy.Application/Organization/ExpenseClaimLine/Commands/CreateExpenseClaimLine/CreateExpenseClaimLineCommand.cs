using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.ExpenseClaimLine.Requests;
using MediatR;

namespace Energy.Application.Organization.ExpenseClaimLine.Commands.CreateExpenseClaimLine;

/// <summary>Yeni ExpenseClaimLine oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateExpenseClaimLineCommand(CreateExpenseClaimLineRequest Request)
    : IRequest<BaseResponse<Guid>>;
