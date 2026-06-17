using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.UnitOfMeasure.Requests;
using MediatR;

namespace Energy.Application.Core.UnitOfMeasure.Commands.CreateUnitOfMeasure;

/// <summary>Yeni UnitOfMeasure oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateUnitOfMeasureCommand(CreateUnitOfMeasureRequest Request)
    : IRequest<BaseResponse<Guid>>;
