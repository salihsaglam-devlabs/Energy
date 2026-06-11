using Energy.Application.Localization.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Localization.Responses;
using MediatR;

namespace Energy.Application.Localization.Queries.GetLocalizationEntries;

public sealed class GetLocalizationEntriesQueryHandler
    : IRequestHandler<GetLocalizationEntriesQuery, BaseResponse<IReadOnlyList<LocalizationEntryResponse>>>
{
    private readonly ILocalizationService _service;

    public GetLocalizationEntriesQueryHandler(ILocalizationService service)
    {
        _service = service;
    }

    public async Task<BaseResponse<IReadOnlyList<LocalizationEntryResponse>>> Handle(
        GetLocalizationEntriesQuery request,
        CancellationToken cancellationToken)
    {
        var entries = await _service.GetAllAsync(cancellationToken);
        return BaseResponse<IReadOnlyList<LocalizationEntryResponse>>.Success(entries);
    }
}

