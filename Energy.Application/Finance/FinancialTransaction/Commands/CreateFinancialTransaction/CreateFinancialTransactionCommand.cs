using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialTransaction.Requests;
using MediatR;

namespace Energy.Application.Finance.FinancialTransaction.Commands.CreateFinancialTransaction;

/// <summary>Yeni FinancialTransaction oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateFinancialTransactionCommand(CreateFinancialTransactionRequest Request)
    : IRequest<BaseResponse<Guid>>;
