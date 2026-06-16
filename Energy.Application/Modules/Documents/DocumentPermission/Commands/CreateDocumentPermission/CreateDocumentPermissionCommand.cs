using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentPermission.Requests;
using MediatR;

namespace Energy.Application.Modules.Documents.DocumentPermission.Commands.CreateDocumentPermission;

/// <summary>Yeni DocumentPermission oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateDocumentPermissionCommand(CreateDocumentPermissionRequest Request)
    : IRequest<BaseResponse<Guid>>;
