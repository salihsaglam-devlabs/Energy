using Energy.Application.Localization.Services;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Localization.Responses;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Energy.Application.Localization.Commands.UpsertLocalizationEntry;

public sealed class UpsertLocalizationEntryCommandHandler
    : IRequestHandler<UpsertLocalizationEntryCommand, BaseResponse<LocalizationEntryResponse>>
{
    private readonly ILocalizationService _service;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public UpsertLocalizationEntryCommandHandler(
        ILocalizationService service,
        IStringLocalizer<SharedResource> localizer)
    {
        _service = service;
        _localizer = localizer;
    }

    public async Task<BaseResponse<LocalizationEntryResponse>> Handle(
        UpsertLocalizationEntryCommand request,
        CancellationToken cancellationToken)
    {
        var entry = await _service.UpsertAsync(request.Request, cancellationToken);
        return BaseResponse<LocalizationEntryResponse>.Success(
            entry,
            _localizer.GetText(LocalizationKeys.LocalizationScreen.EntrySaved, "Localization entry saved."));
    }
}

