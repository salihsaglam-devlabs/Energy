using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.Contract.Responses;
using MediatR;

namespace Energy.Application.Modules.Contracts.Contract.Queries.GetContractLookup;

/// <summary>Contract lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetContractLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<ContractLookupResponse>>>;
