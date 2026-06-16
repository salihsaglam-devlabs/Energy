using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectLocation.Requests;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectLocation.Commands.CreateProjectLocation;

/// <summary>Yeni ProjectLocation oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateProjectLocationCommand(CreateProjectLocationRequest Request)
    : IRequest<BaseResponse<Guid>>;
