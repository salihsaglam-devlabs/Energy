using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Localization.Requests;
using Energy.Shared.Models.V1.Localization.Responses;
using Energy.Application.Localization.Services;
using MediatR;

namespace Energy.Application.Modules.Core.Localization.Commands.DeleteLocalizationEntry;

/// <summary><see cref="DeleteLocalizationEntryCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class DeleteLocalizationEntryCommandHandler
    : IRequestHandler<DeleteLocalizationEntryCommand, BaseResponse<bool>>
{
    private readonly ILocalizationService _localization;

    public DeleteLocalizationEntryCommandHandler(ILocalizationService localization)
    {
        _localization = localization;
    }

    public async Task<BaseResponse<bool>> Handle(DeleteLocalizationEntryCommand request, CancellationToken ct)
    {
        var result = await _localization.DeleteAsync(request.Key, ct);
        return BaseResponse<bool>.Success(result);
    }
}
