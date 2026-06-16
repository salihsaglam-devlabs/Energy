using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Payable.Requests;
using MediatR;

namespace Energy.Application.Finance.Payable.Commands.UpdatePayable;

/// <summary>Var olan Payable kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdatePayableCommand(Guid Id, UpdatePayableRequest Request)
    : IRequest<BaseResponse<bool>>;
