using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Requests.RequestLine.Commands.DeleteRequestLine;

/// <summary>RequestLine kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteRequestLineCommand(Guid Id) : IRequest<BaseResponse<bool>>;
