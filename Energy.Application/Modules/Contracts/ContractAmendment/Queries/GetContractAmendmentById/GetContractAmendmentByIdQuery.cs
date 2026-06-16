using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractAmendment.Responses;
using MediatR;

namespace Energy.Application.Modules.Contracts.ContractAmendment.Queries.GetContractAmendmentById;

/// <summary>Kimliğe göre ContractAmendment detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetContractAmendmentByIdQuery(Guid Id)
    : IRequest<BaseResponse<ContractAmendmentDetailResponse>>;
