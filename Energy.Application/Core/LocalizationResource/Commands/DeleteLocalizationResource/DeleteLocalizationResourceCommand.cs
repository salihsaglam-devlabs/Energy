using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Core.LocalizationResource.Commands.DeleteLocalizationResource;

/// <summary>LocalizationResource kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteLocalizationResourceCommand(Guid Id) : IRequest<BaseResponse<bool>>;
