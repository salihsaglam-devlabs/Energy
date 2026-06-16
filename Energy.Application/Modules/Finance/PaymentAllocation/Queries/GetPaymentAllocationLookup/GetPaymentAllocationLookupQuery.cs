using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.PaymentAllocation.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.PaymentAllocation.Queries.GetPaymentAllocationLookup;

/// <summary>PaymentAllocation lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetPaymentAllocationLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<PaymentAllocationLookupResponse>>>;
