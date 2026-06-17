using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Contracts.ContractParty.Commands.DeleteContractParty;

/// <summary>ContractParty kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteContractPartyCommand(Guid Id) : IRequest<BaseResponse<bool>>;
