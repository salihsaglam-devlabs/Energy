using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Receivable.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.Receivable.Queries.GetReceivableLookup;

/// <summary>Receivable lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetReceivableLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<ReceivableLookupResponse>>>;
