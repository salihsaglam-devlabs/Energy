using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.UnitOfMeasure.Requests;
using MediatR;

namespace Energy.Application.Core.UnitOfMeasure.Commands.UpdateUnitOfMeasure;

/// <summary>Var olan UnitOfMeasure kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateUnitOfMeasureCommand(Guid Id, UpdateUnitOfMeasureRequest Request)
    : IRequest<BaseResponse<bool>>;
