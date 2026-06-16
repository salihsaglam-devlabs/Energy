using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.SequenceDefinition.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.SequenceDefinition.Queries.GetSequenceDefinitionLookup;

/// <summary>SequenceDefinition lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetSequenceDefinitionLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<SequenceDefinitionLookupResponse>>>;
