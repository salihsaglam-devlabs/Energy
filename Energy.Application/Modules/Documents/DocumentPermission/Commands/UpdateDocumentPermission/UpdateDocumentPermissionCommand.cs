using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentPermission.Requests;
using MediatR;

namespace Energy.Application.Modules.Documents.DocumentPermission.Commands.UpdateDocumentPermission;

/// <summary>Var olan DocumentPermission kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateDocumentPermissionCommand(Guid Id, UpdateDocumentPermissionRequest Request)
    : IRequest<BaseResponse<bool>>;
