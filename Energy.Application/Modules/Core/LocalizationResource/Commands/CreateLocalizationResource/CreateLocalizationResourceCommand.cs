using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.LocalizationResource.Requests;
using MediatR;

namespace Energy.Application.Modules.Core.LocalizationResource.Commands.CreateLocalizationResource;

/// <summary>Yeni LocalizationResource oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateLocalizationResourceCommand(CreateLocalizationResourceRequest Request)
    : IRequest<BaseResponse<Guid>>;
