using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.Contract.Requests;
using Energy.Shared.Models.V1.Contracts.Contract.Responses;
using MediatR;

namespace Energy.Application.Modules.Contracts.Contract.Queries.GetContractList;

/// <summary>Sayfalanmış Contract listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetContractListQuery(GetContractListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ContractListResponse>>>;
