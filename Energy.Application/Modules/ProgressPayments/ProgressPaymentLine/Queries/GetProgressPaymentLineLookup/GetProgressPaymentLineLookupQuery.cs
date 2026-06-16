using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentLine.Responses;
using MediatR;

namespace Energy.Application.Modules.ProgressPayments.ProgressPaymentLine.Queries.GetProgressPaymentLineLookup;

/// <summary>ProgressPaymentLine lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetProgressPaymentLineLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<ProgressPaymentLineLookupResponse>>>;
