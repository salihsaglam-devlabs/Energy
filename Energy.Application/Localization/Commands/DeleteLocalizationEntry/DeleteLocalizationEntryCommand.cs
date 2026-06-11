using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Localization.Commands.DeleteLocalizationEntry;

public sealed record DeleteLocalizationEntryCommand(string Key)
    : IRequest<BaseResponse<string>>;

