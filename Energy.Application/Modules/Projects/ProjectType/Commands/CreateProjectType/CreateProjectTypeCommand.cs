using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectType.Requests;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectType.Commands.CreateProjectType;

/// <summary>Yeni ProjectType oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateProjectTypeCommand(CreateProjectTypeRequest Request)
    : IRequest<BaseResponse<Guid>>;
