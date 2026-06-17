using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialTransactionLine.Responses;
using MediatR;

namespace Energy.Application.Finance.FinancialTransactionLine.Queries.GetFinancialTransactionLineById;

/// <summary>Kimliğe göre FinancialTransactionLine detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetFinancialTransactionLineByIdQuery(Guid Id)
    : IRequest<BaseResponse<FinancialTransactionLineDetailResponse>>;
