using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.DocumentPermission.Commands.DeleteDocumentPermission;

/// <summary>DocumentPermission kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteDocumentPermissionCommand(Guid Id) : IRequest<BaseResponse<bool>>;
