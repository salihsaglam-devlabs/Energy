using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.RequestLine.Requests;
using MediatR;

namespace Energy.Application.Modules.Requests.RequestLine.Commands.UpdateRequestLine;

/// <summary>Var olan RequestLine kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateRequestLineCommand(Guid Id, UpdateRequestLineRequest Request)
    : IRequest<BaseResponse<bool>>;
