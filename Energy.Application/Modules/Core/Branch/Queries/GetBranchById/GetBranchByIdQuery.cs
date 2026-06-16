using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Branch.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.Branch.Queries.GetBranchById;

/// <summary>Kimliğe göre Branch detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetBranchByIdQuery(Guid Id)
    : IRequest<BaseResponse<BranchDetailResponse>>;
