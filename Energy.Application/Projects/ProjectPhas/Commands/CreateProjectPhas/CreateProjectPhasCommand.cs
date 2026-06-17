using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectPhas.Requests;
using MediatR;

namespace Energy.Application.Projects.ProjectPhas.Commands.CreateProjectPhas;

/// <summary>Yeni ProjectPhas oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateProjectPhasCommand(CreateProjectPhasRequest Request)
    : IRequest<BaseResponse<Guid>>;
