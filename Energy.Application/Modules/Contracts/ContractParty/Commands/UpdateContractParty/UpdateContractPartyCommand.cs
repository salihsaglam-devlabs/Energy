using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractParty.Requests;
using MediatR;

namespace Energy.Application.Modules.Contracts.ContractParty.Commands.UpdateContractParty;

/// <summary>Var olan ContractParty kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateContractPartyCommand(Guid Id, UpdateContractPartyRequest Request)
    : IRequest<BaseResponse<bool>>;
