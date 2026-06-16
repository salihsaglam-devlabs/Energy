using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.Project.Requests;
using MediatR;

namespace Energy.Application.Projects.Project.Commands.CreateProject;

/// <summary>Yeni Project oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateProjectCommand(CreateProjectRequest Request)
    : IRequest<BaseResponse<Guid>>;
