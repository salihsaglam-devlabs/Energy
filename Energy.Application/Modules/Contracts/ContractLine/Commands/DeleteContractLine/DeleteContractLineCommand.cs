using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Contracts.ContractLine.Commands.DeleteContractLine;

/// <summary>ContractLine kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteContractLineCommand(Guid Id) : IRequest<BaseResponse<bool>>;
