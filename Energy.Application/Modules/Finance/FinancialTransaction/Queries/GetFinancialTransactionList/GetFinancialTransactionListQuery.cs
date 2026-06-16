using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialTransaction.Requests;
using Energy.Shared.Models.V1.Finance.FinancialTransaction.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.FinancialTransaction.Queries.GetFinancialTransactionList;

/// <summary>Sayfalanmış FinancialTransaction listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetFinancialTransactionListQuery(GetFinancialTransactionListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<FinancialTransactionListResponse>>>;
