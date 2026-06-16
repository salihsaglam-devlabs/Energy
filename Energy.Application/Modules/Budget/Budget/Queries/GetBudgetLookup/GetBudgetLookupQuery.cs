using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Budget.Budget.Responses;
using MediatR;

namespace Energy.Application.Modules.Budget.Budget.Queries.GetBudgetLookup;

/// <summary>Budget lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetBudgetLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<BudgetLookupResponse>>>;
