using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.Seeding.Commands.SeedLocalizationFromResx;

/// <summary>SeedLocalizationFromResx</summary>
public sealed record SeedLocalizationFromResxCommand()
    : IRequest<BaseResponse<SeedResultResponse>>;
