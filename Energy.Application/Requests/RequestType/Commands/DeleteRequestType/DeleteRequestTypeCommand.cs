using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Requests.RequestType.Commands.DeleteRequestType;

/// <summary>RequestType kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteRequestTypeCommand(Guid Id) : IRequest<BaseResponse<bool>>;
