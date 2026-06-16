using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialUnitConversion.Requests;
using MediatR;

namespace Energy.Application.Catalog.MaterialUnitConversion.Commands.CreateMaterialUnitConversion;

/// <summary>Yeni MaterialUnitConversion oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateMaterialUnitConversionCommand(CreateMaterialUnitConversionRequest Request)
    : IRequest<BaseResponse<Guid>>;
