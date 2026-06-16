using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Reports.PayableAging.Requests;
using Energy.Shared.Models.V1.Finance.Reports.PayableAging.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.Reports.PayableAging.Queries.GetPayableAgingData;

/// <summary>PayableAging rapor verisi (filtreli, sayfalı).</summary>
public sealed record GetPayableAgingDataQuery(PayableAgingRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<PayableAgingRowResponse>>>;
