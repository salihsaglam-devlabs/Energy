using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentDeduction.Requests;
using MediatR;

namespace Energy.Application.ProgressPayments.ProgressPaymentDeduction.Commands.CreateProgressPaymentDeduction;

/// <summary>Yeni ProgressPaymentDeduction oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateProgressPaymentDeductionCommand(CreateProgressPaymentDeductionRequest Request)
    : IRequest<BaseResponse<Guid>>;
