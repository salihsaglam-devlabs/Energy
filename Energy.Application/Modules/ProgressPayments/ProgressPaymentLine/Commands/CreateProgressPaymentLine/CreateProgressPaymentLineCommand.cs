using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentLine.Requests;
using MediatR;

namespace Energy.Application.Modules.ProgressPayments.ProgressPaymentLine.Commands.CreateProgressPaymentLine;

/// <summary>Yeni ProgressPaymentLine oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateProgressPaymentLineCommand(CreateProgressPaymentLineRequest Request)
    : IRequest<BaseResponse<Guid>>;
