using Energy.Application.Localization.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Localization.Commands.ImportLocalizationFromResx;

public sealed class ImportLocalizationFromResxCommandHandler
    : IRequestHandler<ImportLocalizationFromResxCommand, BaseResponse<SeedResultResponse>>
{
    private readonly ILocalizationService _service;

    public ImportLocalizationFromResxCommandHandler(ILocalizationService service)
    {
        _service = service;
    }

    public async Task<BaseResponse<SeedResultResponse>> Handle(
        ImportLocalizationFromResxCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.ImportFromResxAsync(cancellationToken);
        return BaseResponse<SeedResultResponse>.Success(result);
    }
}

