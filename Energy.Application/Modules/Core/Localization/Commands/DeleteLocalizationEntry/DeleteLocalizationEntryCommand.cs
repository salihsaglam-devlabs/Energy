using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Localization.Requests;
using Energy.Shared.Models.V1.Localization.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.Localization.Commands.DeleteLocalizationEntry;

/// <summary>DeleteLocalizationEntry</summary>
public sealed record DeleteLocalizationEntryCommand(string Key)
    : IRequest<BaseResponse<bool>>;
