using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Payment.Requests;
using MediatR;

namespace Energy.Application.Modules.Finance.Payment.Commands.CreatePayment;

/// <summary>Yeni Payment oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreatePaymentCommand(CreatePaymentRequest Request)
    : IRequest<BaseResponse<Guid>>;
