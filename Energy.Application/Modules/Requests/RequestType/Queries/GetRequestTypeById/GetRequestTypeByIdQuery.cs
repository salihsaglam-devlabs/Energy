using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.RequestType.Responses;
using MediatR;

namespace Energy.Application.Modules.Requests.RequestType.Queries.GetRequestTypeById;

/// <summary>Kimliğe göre RequestType detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetRequestTypeByIdQuery(Guid Id)
    : IRequest<BaseResponse<RequestTypeDetailResponse>>;
