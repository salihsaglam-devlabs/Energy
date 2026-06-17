using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Localization.Requests;
using Energy.Shared.Models.V1.Localization.Responses;
using MediatR;

namespace Energy.Application.Core.Localization.Queries.GetLocalizationEntries;

/// <summary>GetLocalizationEntries</summary>
public sealed record GetLocalizationEntriesQuery()
    : IRequest<BaseResponse<IReadOnlyList<LocalizationEntryResponse>>>;
