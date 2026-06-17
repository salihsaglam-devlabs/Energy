using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Core.Seeding.Commands.SeedLocalization;

/// <summary>SeedLocalization</summary>
public sealed record SeedLocalizationCommand()
    : IRequest<BaseResponse<SeedResultResponse>>;
