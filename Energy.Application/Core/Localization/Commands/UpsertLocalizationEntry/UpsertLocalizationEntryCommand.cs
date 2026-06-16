using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Localization.Requests;
using Energy.Shared.Models.V1.Localization.Responses;
using MediatR;

namespace Energy.Application.Core.Localization.Commands.UpsertLocalizationEntry;

/// <summary>UpsertLocalizationEntry</summary>
public sealed record UpsertLocalizationEntryCommand(UpsertLocalizationEntryRequest Request)
    : IRequest<BaseResponse<LocalizationEntryResponse>>;
