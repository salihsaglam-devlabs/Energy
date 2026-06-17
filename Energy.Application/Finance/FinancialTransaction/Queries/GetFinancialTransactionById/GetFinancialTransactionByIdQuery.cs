using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialTransaction.Responses;
using MediatR;

namespace Energy.Application.Finance.FinancialTransaction.Queries.GetFinancialTransactionById;

/// <summary>Kimliğe göre FinancialTransaction detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetFinancialTransactionByIdQuery(Guid Id)
    : IRequest<BaseResponse<FinancialTransactionDetailResponse>>;
