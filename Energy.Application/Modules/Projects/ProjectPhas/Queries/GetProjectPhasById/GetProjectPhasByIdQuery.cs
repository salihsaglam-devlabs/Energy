using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectPhas.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectPhas.Queries.GetProjectPhasById;

/// <summary>Kimliğe göre ProjectPhas detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetProjectPhasByIdQuery(Guid Id)
    : IRequest<BaseResponse<ProjectPhasDetailResponse>>;
