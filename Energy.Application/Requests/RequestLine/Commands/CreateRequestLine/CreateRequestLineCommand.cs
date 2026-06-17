using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.RequestLine.Requests;
using MediatR;

namespace Energy.Application.Requests.RequestLine.Commands.CreateRequestLine;

/// <summary>Yeni RequestLine oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateRequestLineCommand(CreateRequestLineRequest Request)
    : IRequest<BaseResponse<Guid>>;
