using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialAccount.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.FinancialAccount.Queries.GetFinancialAccountById;

/// <summary>Kimliğe göre FinancialAccount detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetFinancialAccountByIdQuery(Guid Id)
    : IRequest<BaseResponse<FinancialAccountDetailResponse>>;
