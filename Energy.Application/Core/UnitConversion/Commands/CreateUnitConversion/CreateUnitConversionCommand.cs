using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.UnitConversion.Requests;
using MediatR;

namespace Energy.Application.Core.UnitConversion.Commands.CreateUnitConversion;

/// <summary>Yeni UnitConversion oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateUnitConversionCommand(CreateUnitConversionRequest Request)
    : IRequest<BaseResponse<Guid>>;
