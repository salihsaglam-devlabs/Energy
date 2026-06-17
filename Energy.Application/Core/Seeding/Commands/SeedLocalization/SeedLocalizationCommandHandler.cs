using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Localization.Services;
using MediatR;

namespace Energy.Application.Core.Seeding.Commands.SeedLocalization;

/// <summary><see cref="SeedLocalizationCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class SeedLocalizationCommandHandler
    : IRequestHandler<SeedLocalizationCommand, BaseResponse<SeedResultResponse>>
{
    private readonly ILocalizationService _localization;

    public SeedLocalizationCommandHandler(ILocalizationService localization)
    {
        _localization = localization;
    }

    public async Task<BaseResponse<SeedResultResponse>> Handle(SeedLocalizationCommand request, CancellationToken ct)
    {
        var result = await _localization.SeedFromResourcesAsync(ct);
        return BaseResponse<SeedResultResponse>.Success(result);
    }
}
