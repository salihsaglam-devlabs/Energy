using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialUnitConversion.Requests;
using MediatR;

namespace Energy.Application.Catalog.MaterialUnitConversion.Commands.UpdateMaterialUnitConversion;

/// <summary>Var olan MaterialUnitConversion kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateMaterialUnitConversionCommand(Guid Id, UpdateMaterialUnitConversionRequest Request)
    : IRequest<BaseResponse<bool>>;
