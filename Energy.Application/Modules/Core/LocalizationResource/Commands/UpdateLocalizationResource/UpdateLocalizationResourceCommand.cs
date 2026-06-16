using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.LocalizationResource.Requests;
using MediatR;

namespace Energy.Application.Modules.Core.LocalizationResource.Commands.UpdateLocalizationResource;

/// <summary>Var olan LocalizationResource kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateLocalizationResourceCommand(Guid Id, UpdateLocalizationResourceRequest Request)
    : IRequest<BaseResponse<bool>>;
