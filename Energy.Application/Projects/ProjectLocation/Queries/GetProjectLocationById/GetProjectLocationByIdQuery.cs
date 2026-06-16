using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectLocation.Responses;
using MediatR;

namespace Energy.Application.Projects.ProjectLocation.Queries.GetProjectLocationById;

/// <summary>Kimliğe göre ProjectLocation detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetProjectLocationByIdQuery(Guid Id)
    : IRequest<BaseResponse<ProjectLocationDetailResponse>>;
