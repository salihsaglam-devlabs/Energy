using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Requests.Request.Commands.DeleteRequest;

/// <summary>Request kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteRequestCommand(Guid Id) : IRequest<BaseResponse<bool>>;
