using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Localization.Responses;
using MediatR;

namespace Energy.Application.Localization.Queries.GetLocalizationEntryByKey;

public sealed record GetLocalizationEntryByKeyQuery(string Key)
    : IRequest<BaseResponse<LocalizationEntryResponse>>;

