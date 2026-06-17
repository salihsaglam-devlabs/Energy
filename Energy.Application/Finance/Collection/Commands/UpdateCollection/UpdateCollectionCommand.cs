using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Collection.Requests;
using MediatR;

namespace Energy.Application.Finance.Collection.Commands.UpdateCollection;

/// <summary>Var olan Collection kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateCollectionCommand(Guid Id, UpdateCollectionRequest Request)
    : IRequest<BaseResponse<bool>>;
