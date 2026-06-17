using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.RequestType.Requests;
using MediatR;

namespace Energy.Application.Requests.RequestType.Commands.UpdateRequestType;

/// <summary>Var olan RequestType kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateRequestTypeCommand(Guid Id, UpdateRequestTypeRequest Request)
    : IRequest<BaseResponse<bool>>;
