using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Receivable.Requests;
using MediatR;

namespace Energy.Application.Finance.Receivable.Commands.UpdateReceivable;

/// <summary>Var olan Receivable kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateReceivableCommand(Guid Id, UpdateReceivableRequest Request)
    : IRequest<BaseResponse<bool>>;
