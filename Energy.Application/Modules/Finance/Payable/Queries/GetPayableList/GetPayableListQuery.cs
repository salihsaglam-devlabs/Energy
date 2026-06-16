using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Payable.Requests;
using Energy.Shared.Models.V1.Finance.Payable.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.Payable.Queries.GetPayableList;

/// <summary>Sayfalanmış Payable listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetPayableListQuery(GetPayableListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<PayableListResponse>>>;
