using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Localization.Requests;
using Energy.Shared.Models.V1.Localization.Responses;
using Energy.Application.Localization.Services;
using MediatR;

namespace Energy.Application.Modules.Core.Localization.Commands.UpsertLocalizationEntry;

/// <summary><see cref="UpsertLocalizationEntryCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class UpsertLocalizationEntryCommandHandler
    : IRequestHandler<UpsertLocalizationEntryCommand, BaseResponse<LocalizationEntryResponse>>
{
    private readonly ILocalizationService _localization;

    public UpsertLocalizationEntryCommandHandler(ILocalizationService localization)
    {
        _localization = localization;
    }

    public async Task<BaseResponse<LocalizationEntryResponse>> Handle(UpsertLocalizationEntryCommand request, CancellationToken ct)
    {
        var result = await _localization.UpsertAsync(request.Request, ct);
        return BaseResponse<LocalizationEntryResponse>.Success(result);
    }
}
