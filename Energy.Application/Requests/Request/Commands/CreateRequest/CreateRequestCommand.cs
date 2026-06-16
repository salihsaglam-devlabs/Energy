using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.Request.Requests;
using MediatR;

namespace Energy.Application.Requests.Request.Commands.CreateRequest;

/// <summary>Yeni Request oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateRequestCommand(CreateRequestRequest Request)
    : IRequest<BaseResponse<Guid>>;
