using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialUnitConversion.Commands.DeleteMaterialUnitConversion;

/// <summary>MaterialUnitConversion kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteMaterialUnitConversionCommand(Guid Id) : IRequest<BaseResponse<bool>>;
