using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.Project.Responses;
using MediatR;

namespace Energy.Application.Projects.Project.Queries.GetProjectById;

/// <summary>Kimliğe göre Project detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetProjectByIdQuery(Guid Id)
    : IRequest<BaseResponse<ProjectDetailResponse>>;
