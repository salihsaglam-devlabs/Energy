using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.CostCenter.Responses;
using MediatR;

namespace Energy.Application.Finance.CostCenter.Queries.GetCostCenterById;

/// <summary>Kimliğe göre CostCenter detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetCostCenterByIdQuery(Guid Id)
    : IRequest<BaseResponse<CostCenterDetailResponse>>;
