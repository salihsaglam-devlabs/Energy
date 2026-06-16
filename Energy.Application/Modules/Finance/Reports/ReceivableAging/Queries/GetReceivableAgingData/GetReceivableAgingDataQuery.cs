using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Reports.ReceivableAging.Requests;
using Energy.Shared.Models.V1.Finance.Reports.ReceivableAging.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.Reports.ReceivableAging.Queries.GetReceivableAgingData;

/// <summary>ReceivableAging rapor verisi (filtreli, sayfalı).</summary>
public sealed record GetReceivableAgingDataQuery(ReceivableAgingRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ReceivableAgingRowResponse>>>;
