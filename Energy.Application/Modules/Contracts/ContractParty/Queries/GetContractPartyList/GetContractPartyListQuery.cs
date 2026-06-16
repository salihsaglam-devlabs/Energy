using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractParty.Requests;
using Energy.Shared.Models.V1.Contracts.ContractParty.Responses;
using MediatR;

namespace Energy.Application.Modules.Contracts.ContractParty.Queries.GetContractPartyList;

/// <summary>Sayfalanmış ContractParty listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetContractPartyListQuery(GetContractPartyListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ContractPartyListResponse>>>;
