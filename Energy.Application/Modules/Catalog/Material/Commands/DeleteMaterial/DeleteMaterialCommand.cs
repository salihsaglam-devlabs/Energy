using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.Material.Commands.DeleteMaterial;

/// <summary>Material kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteMaterialCommand(Guid Id) : IRequest<BaseResponse<bool>>;
