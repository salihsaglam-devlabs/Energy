using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractParty.Responses;
using MediatR;

namespace Energy.Application.Contracts.ContractParty.Queries.GetContractPartyById;

/// <summary>Kimliğe göre ContractParty detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetContractPartyByIdQuery(Guid Id)
    : IRequest<BaseResponse<ContractPartyDetailResponse>>;
