using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Budget.Budget.Requests;
using MediatR;

namespace Energy.Application.Budget.Budget.Commands.CreateBudget;

/// <summary>Yeni Budget oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateBudgetCommand(CreateBudgetRequest Request)
    : IRequest<BaseResponse<Guid>>;
