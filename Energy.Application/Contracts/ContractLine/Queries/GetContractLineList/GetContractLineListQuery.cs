using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractLine.Requests;
using Energy.Shared.Models.V1.Contracts.ContractLine.Responses;
using MediatR;

namespace Energy.Application.Contracts.ContractLine.Queries.GetContractLineList;

/// <summary>Sayfalanmış ContractLine listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetContractLineListQuery(GetContractLineListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ContractLineListResponse>>>;
