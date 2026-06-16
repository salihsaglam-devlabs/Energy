using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.RequestLine.Responses;
using MediatR;

namespace Energy.Application.Requests.RequestLine.Queries.GetRequestLineById;

/// <summary>Kimliğe göre RequestLine detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetRequestLineByIdQuery(Guid Id)
    : IRequest<BaseResponse<RequestLineDetailResponse>>;
