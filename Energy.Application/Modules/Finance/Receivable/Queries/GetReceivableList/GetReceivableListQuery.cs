using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Receivable.Requests;
using Energy.Shared.Models.V1.Finance.Receivable.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.Receivable.Queries.GetReceivableList;

/// <summary>Sayfalanmış Receivable listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetReceivableListQuery(GetReceivableListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ReceivableListResponse>>>;
