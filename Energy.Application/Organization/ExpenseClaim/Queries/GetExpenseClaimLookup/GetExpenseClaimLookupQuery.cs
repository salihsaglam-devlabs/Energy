using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.ExpenseClaim.Responses;
using MediatR;

namespace Energy.Application.Organization.ExpenseClaim.Queries.GetExpenseClaimLookup;

/// <summary>ExpenseClaim lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetExpenseClaimLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<ExpenseClaimLookupResponse>>>;
