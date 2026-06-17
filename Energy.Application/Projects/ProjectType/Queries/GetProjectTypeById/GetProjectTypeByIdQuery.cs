using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectType.Responses;
using MediatR;

namespace Energy.Application.Projects.ProjectType.Queries.GetProjectTypeById;

/// <summary>Kimliğe göre ProjectType detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetProjectTypeByIdQuery(Guid Id)
    : IRequest<BaseResponse<ProjectTypeDetailResponse>>;
