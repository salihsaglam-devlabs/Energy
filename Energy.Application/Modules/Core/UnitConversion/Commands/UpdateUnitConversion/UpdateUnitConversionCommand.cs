using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.UnitConversion.Requests;
using MediatR;

namespace Energy.Application.Modules.Core.UnitConversion.Commands.UpdateUnitConversion;

/// <summary>Var olan UnitConversion kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateUnitConversionCommand(Guid Id, UpdateUnitConversionRequest Request)
    : IRequest<BaseResponse<bool>>;
