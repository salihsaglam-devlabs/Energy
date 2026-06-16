using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Localization.Requests;
using Energy.Shared.Models.V1.Localization.Responses;
using Energy.Application.Localization.Services;
using MediatR;

namespace Energy.Application.Core.Localization.Queries.GetLocalizationByKey;

/// <summary><see cref="GetLocalizationByKeyQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetLocalizationByKeyQueryHandler
    : IRequestHandler<GetLocalizationByKeyQuery, BaseResponse<LocalizationEntryResponse>>
{
    private readonly ILocalizationService _localization;

    public GetLocalizationByKeyQueryHandler(ILocalizationService localization)
    {
        _localization = localization;
    }

    public async Task<BaseResponse<LocalizationEntryResponse>> Handle(GetLocalizationByKeyQuery request, CancellationToken ct)
    {
        var result = await _localization.GetByKeyAsync(request.Key, ct);
        return result is null
            ? BaseResponse<LocalizationEntryResponse>.Failure("Key not found.")
            : BaseResponse<LocalizationEntryResponse>.Success(result);
    }
}
