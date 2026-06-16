using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPayment.Requests;
using MediatR;

namespace Energy.Application.Modules.ProgressPayments.ProgressPayment.Commands.CreateProgressPayment;

/// <summary>Yeni ProgressPayment oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateProgressPaymentCommand(CreateProgressPaymentRequest Request)
    : IRequest<BaseResponse<Guid>>;
