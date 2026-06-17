using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.PaymentAllocation.Requests;
using MediatR;

namespace Energy.Application.Finance.PaymentAllocation.Commands.CreatePaymentAllocation;

/// <summary>Yeni PaymentAllocation oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreatePaymentAllocationCommand(CreatePaymentAllocationRequest Request)
    : IRequest<BaseResponse<Guid>>;
