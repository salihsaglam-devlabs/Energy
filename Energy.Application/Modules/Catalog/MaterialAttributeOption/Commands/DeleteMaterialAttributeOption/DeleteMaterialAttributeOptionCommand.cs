using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialAttributeOption.Commands.DeleteMaterialAttributeOption;

/// <summary>MaterialAttributeOption kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteMaterialAttributeOptionCommand(Guid Id) : IRequest<BaseResponse<bool>>;
