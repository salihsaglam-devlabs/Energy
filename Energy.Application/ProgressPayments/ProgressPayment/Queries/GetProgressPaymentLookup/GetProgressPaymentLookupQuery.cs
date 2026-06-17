using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPayment.Responses;
using MediatR;

namespace Energy.Application.ProgressPayments.ProgressPayment.Queries.GetProgressPaymentLookup;

/// <summary>ProgressPayment lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetProgressPaymentLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<ProgressPaymentLookupResponse>>>;
