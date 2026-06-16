using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.PaymentAllocation.Requests;
using MediatR;

namespace Energy.Application.Modules.Finance.PaymentAllocation.Commands.UpdatePaymentAllocation;

/// <summary>Var olan PaymentAllocation kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdatePaymentAllocationCommand(Guid Id, UpdatePaymentAllocationRequest Request)
    : IRequest<BaseResponse<bool>>;
