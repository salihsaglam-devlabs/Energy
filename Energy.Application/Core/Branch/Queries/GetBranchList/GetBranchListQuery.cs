using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Branch.Requests;
using Energy.Shared.Models.V1.Core.Branch.Responses;
using MediatR;

namespace Energy.Application.Core.Branch.Queries.GetBranchList;

/// <summary>Sayfalanmış Branch listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetBranchListQuery(GetBranchListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<BranchListResponse>>>;
