using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Localization.Requests;
using Energy.Shared.Models.V1.Localization.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.Localization.Queries.GetLocalizationByKey;

/// <summary>GetLocalizationByKey</summary>
public sealed record GetLocalizationByKeyQuery(string Key)
    : IRequest<BaseResponse<LocalizationEntryResponse>>;
