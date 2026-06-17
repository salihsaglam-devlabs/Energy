using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Finance.Receivable.Commands.DeleteReceivable;

/// <summary>Receivable kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteReceivableCommand(Guid Id) : IRequest<BaseResponse<bool>>;
