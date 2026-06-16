using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.UnitConversion.Commands.DeleteUnitConversion;

/// <summary>UnitConversion kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteUnitConversionCommand(Guid Id) : IRequest<BaseResponse<bool>>;
