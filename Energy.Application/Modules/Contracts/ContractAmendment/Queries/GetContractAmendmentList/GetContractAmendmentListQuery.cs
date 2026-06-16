using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractAmendment.Requests;
using Energy.Shared.Models.V1.Contracts.ContractAmendment.Responses;
using MediatR;

namespace Energy.Application.Modules.Contracts.ContractAmendment.Queries.GetContractAmendmentList;

/// <summary>Sayfalanmış ContractAmendment listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetContractAmendmentListQuery(GetContractAmendmentListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ContractAmendmentListResponse>>>;
