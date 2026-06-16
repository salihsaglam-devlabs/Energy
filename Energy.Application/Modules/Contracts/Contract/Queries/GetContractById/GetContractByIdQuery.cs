using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.Contract.Responses;
using MediatR;

namespace Energy.Application.Modules.Contracts.Contract.Queries.GetContractById;

/// <summary>Kimliğe göre Contract detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetContractByIdQuery(Guid Id)
    : IRequest<BaseResponse<ContractDetailResponse>>;
