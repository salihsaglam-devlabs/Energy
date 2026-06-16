using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Receivable.Responses;
using MediatR;

namespace Energy.Application.Finance.Receivable.Queries.GetReceivableById;

/// <summary>Kimliğe göre Receivable detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetReceivableByIdQuery(Guid Id)
    : IRequest<BaseResponse<ReceivableDetailResponse>>;
