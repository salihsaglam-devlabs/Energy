using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialTransactionLine.Requests;
using Energy.Shared.Models.V1.Finance.FinancialTransactionLine.Responses;
using MediatR;

namespace Energy.Application.Finance.FinancialTransactionLine.Queries.GetFinancialTransactionLineList;

/// <summary>Sayfalanmış FinancialTransactionLine listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetFinancialTransactionLineListQuery(GetFinancialTransactionLineListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<FinancialTransactionLineListResponse>>>;
