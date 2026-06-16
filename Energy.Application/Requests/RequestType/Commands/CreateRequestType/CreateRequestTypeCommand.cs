using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.RequestType.Requests;
using MediatR;

namespace Energy.Application.Requests.RequestType.Commands.CreateRequestType;

/// <summary>Yeni RequestType oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateRequestTypeCommand(CreateRequestTypeRequest Request)
    : IRequest<BaseResponse<Guid>>;
