using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.UnitOfMeasure.Commands.DeleteUnitOfMeasure;

/// <summary>UnitOfMeasure kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteUnitOfMeasureCommand(Guid Id) : IRequest<BaseResponse<bool>>;
