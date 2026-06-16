using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.LocalizationResource.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.LocalizationResource.Queries.GetLocalizationResourceById;

/// <summary>Kimliğe göre LocalizationResource detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetLocalizationResourceByIdQuery(Guid Id)
    : IRequest<BaseResponse<LocalizationResourceDetailResponse>>;
