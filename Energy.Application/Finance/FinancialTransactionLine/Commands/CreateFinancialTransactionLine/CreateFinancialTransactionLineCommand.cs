using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialTransactionLine.Requests;
using MediatR;

namespace Energy.Application.Finance.FinancialTransactionLine.Commands.CreateFinancialTransactionLine;

/// <summary>Yeni FinancialTransactionLine oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateFinancialTransactionLineCommand(CreateFinancialTransactionLineRequest Request)
    : IRequest<BaseResponse<Guid>>;
