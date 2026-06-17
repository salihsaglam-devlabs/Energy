using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Localization.Requests;
using Energy.Shared.Models.V1.Localization.Responses;
using Energy.Application.Localization.Services;
using MediatR;

namespace Energy.Application.Core.Localization.Queries.GetLocalizationEntries;

/// <summary><see cref="GetLocalizationEntriesQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetLocalizationEntriesQueryHandler
    : IRequestHandler<GetLocalizationEntriesQuery, BaseResponse<IReadOnlyList<LocalizationEntryResponse>>>
{
    private readonly ILocalizationService _localization;

    public GetLocalizationEntriesQueryHandler(ILocalizationService localization)
    {
        _localization = localization;
    }

    public async Task<BaseResponse<IReadOnlyList<LocalizationEntryResponse>>> Handle(GetLocalizationEntriesQuery request, CancellationToken ct)
    {
        var result = await _localization.GetAllAsync(ct);
        return BaseResponse<IReadOnlyList<LocalizationEntryResponse>>.Success(result);
    }
}
