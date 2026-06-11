using Energy.Application.Common.Exceptions;
using Energy.Application.Localization.Services;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Energy.Application.Localization.Commands.DeleteLocalizationEntry;

public sealed class DeleteLocalizationEntryCommandHandler
    : IRequestHandler<DeleteLocalizationEntryCommand, BaseResponse<string>>
{
    private readonly ILocalizationService _service;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public DeleteLocalizationEntryCommandHandler(
        ILocalizationService service,
        IStringLocalizer<SharedResource> localizer)
    {
        _service = service;
        _localizer = localizer;
    }

    public async Task<BaseResponse<string>> Handle(
        DeleteLocalizationEntryCommand request,
        CancellationToken cancellationToken)
    {
        var deleted = await _service.DeleteAsync(request.Key, cancellationToken);
        if (!deleted)
        {
            throw new NotFoundException(string.Format(
                _localizer.GetText(LocalizationKeys.LocalizationScreen.KeyNotFound, "Localization key '{0}' was not found."),
                request.Key));
        }

        return BaseResponse<string>.Success(
            request.Key,
            _localizer.GetText(LocalizationKeys.LocalizationScreen.EntryDeleted, "Localization entry deleted."));
    }
}

