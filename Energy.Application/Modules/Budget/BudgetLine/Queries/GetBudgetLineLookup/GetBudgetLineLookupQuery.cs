using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Budget.BudgetLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Budget.BudgetLine.Queries.GetBudgetLineLookup;

/// <summary>BudgetLine lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetBudgetLineLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<BudgetLineLookupResponse>>>;
