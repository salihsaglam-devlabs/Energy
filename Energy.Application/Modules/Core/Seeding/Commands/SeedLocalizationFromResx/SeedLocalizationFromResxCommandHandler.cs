using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Localization.Services;
using MediatR;

namespace Energy.Application.Modules.Core.Seeding.Commands.SeedLocalizationFromResx;

/// <summary><see cref="SeedLocalizationFromResxCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class SeedLocalizationFromResxCommandHandler
    : IRequestHandler<SeedLocalizationFromResxCommand, BaseResponse<SeedResultResponse>>
{
    private readonly ILocalizationService _localization;

    public SeedLocalizationFromResxCommandHandler(ILocalizationService localization)
    {
        _localization = localization;
    }

    public async Task<BaseResponse<SeedResultResponse>> Handle(SeedLocalizationFromResxCommand request, CancellationToken ct)
    {
        var result = await _localization.ImportFromResxAsync(ct);
        return BaseResponse<SeedResultResponse>.Success(result);
    }
}
