using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Payable.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.Payable.Queries.GetPayableById;

/// <summary>Kimliğe göre Payable detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetPayableByIdQuery(Guid Id)
    : IRequest<BaseResponse<PayableDetailResponse>>;
