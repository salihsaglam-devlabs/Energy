using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Receivable.Requests;
using MediatR;

namespace Energy.Application.Finance.Receivable.Commands.CreateReceivable;

/// <summary>Yeni Receivable oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateReceivableCommand(CreateReceivableRequest Request)
    : IRequest<BaseResponse<Guid>>;
