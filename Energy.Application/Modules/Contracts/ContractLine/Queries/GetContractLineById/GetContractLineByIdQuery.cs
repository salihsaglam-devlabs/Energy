using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Contracts.ContractLine.Queries.GetContractLineById;

/// <summary>Kimliğe göre ContractLine detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetContractLineByIdQuery(Guid Id)
    : IRequest<BaseResponse<ContractLineDetailResponse>>;
