using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialTransactionLine.Responses;
using MediatR;

namespace Energy.Application.Finance.FinancialTransactionLine.Queries.GetFinancialTransactionLineLookup;

/// <summary>FinancialTransactionLine lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetFinancialTransactionLineLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<FinancialTransactionLineLookupResponse>>>;
