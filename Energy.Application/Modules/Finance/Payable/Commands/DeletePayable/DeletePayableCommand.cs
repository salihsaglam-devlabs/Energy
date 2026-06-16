using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.Payable.Commands.DeletePayable;

/// <summary>Payable kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeletePayableCommand(Guid Id) : IRequest<BaseResponse<bool>>;
