using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.Request.Responses;
using MediatR;

namespace Energy.Application.Requests.Request.Queries.GetRequestById;

/// <summary>Kimliğe göre Request detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetRequestByIdQuery(Guid Id)
    : IRequest<BaseResponse<RequestDetailResponse>>;
