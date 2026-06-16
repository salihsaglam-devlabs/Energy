using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectStatus.Requests;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectStatus.Commands.CreateProjectStatus;

/// <summary>Yeni ProjectStatus oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateProjectStatusCommand(CreateProjectStatusRequest Request)
    : IRequest<BaseResponse<Guid>>;
