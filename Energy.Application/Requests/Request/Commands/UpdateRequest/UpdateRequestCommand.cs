using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.Request.Requests;
using MediatR;

namespace Energy.Application.Requests.Request.Commands.UpdateRequest;

/// <summary>Var olan Request kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateRequestCommand(Guid Id, UpdateRequestRequest Request)
    : IRequest<BaseResponse<bool>>;
