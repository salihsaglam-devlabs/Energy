using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Localization.Responses;
using MediatR;

namespace Energy.Application.Localization.Queries.GetLocalizationEntries;

public sealed record GetLocalizationEntriesQuery
    : IRequest<BaseResponse<IReadOnlyList<LocalizationEntryResponse>>>;

