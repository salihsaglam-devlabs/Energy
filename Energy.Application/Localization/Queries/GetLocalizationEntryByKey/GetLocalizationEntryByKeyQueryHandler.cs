using Energy.Application.Common.Exceptions;
using Energy.Application.Localization.Services;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Localization.Responses;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Energy.Application.Localization.Queries.GetLocalizationEntryByKey;

public sealed class GetLocalizationEntryByKeyQueryHandler
    : IRequestHandler<GetLocalizationEntryByKeyQuery, BaseResponse<LocalizationEntryResponse>>
{
    private readonly ILocalizationService _service;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public GetLocalizationEntryByKeyQueryHandler(
        ILocalizationService service,
        IStringLocalizer<SharedResource> localizer)
    {
        _service = service;
        _localizer = localizer;
    }

    public async Task<BaseResponse<LocalizationEntryResponse>> Handle(
        GetLocalizationEntryByKeyQuery request,
        CancellationToken cancellationToken)
    {
        var entry = await _service.GetByKeyAsync(request.Key, cancellationToken)
                    ?? throw new NotFoundException(string.Format(
                        _localizer.GetText(LocalizationKeys.Messages.LocalizationKeyNotFound, "Localization key '{0}' was not found."),
                        request.Key));

        return BaseResponse<LocalizationEntryResponse>.Success(entry);
    }
}

