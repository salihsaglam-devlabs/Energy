using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialAccount.Requests;
using Energy.Shared.Models.V1.Finance.FinancialAccount.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.FinancialAccount.Queries.GetFinancialAccountList;

/// <summary>Sayfalanmış FinancialAccount listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetFinancialAccountListQuery(GetFinancialAccountListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<FinancialAccountListResponse>>>;
