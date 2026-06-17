using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Budget.BudgetLine.Requests;
using MediatR;

namespace Energy.Application.Budget.BudgetLine.Commands.CreateBudgetLine;

/// <summary>Yeni BudgetLine oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateBudgetLineCommand(CreateBudgetLineRequest Request)
    : IRequest<BaseResponse<Guid>>;
