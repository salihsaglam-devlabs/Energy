using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectStatus.Responses;
using MediatR;

namespace Energy.Application.Projects.ProjectStatus.Queries.GetProjectStatusById;

/// <summary>Kimliğe göre ProjectStatus detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetProjectStatusByIdQuery(Guid Id)
    : IRequest<BaseResponse<ProjectStatusDetailResponse>>;
