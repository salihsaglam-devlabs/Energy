using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Localization.Commands.ImportLocalizationFromResx;

public sealed record ImportLocalizationFromResxCommand
    : IRequest<BaseResponse<SeedResultResponse>>;

