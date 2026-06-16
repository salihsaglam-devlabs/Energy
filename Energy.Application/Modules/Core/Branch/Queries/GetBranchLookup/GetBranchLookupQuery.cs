using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Branch.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.Branch.Queries.GetBranchLookup;

/// <summary>Branch lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetBranchLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<BranchLookupResponse>>>;
