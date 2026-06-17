using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractParty.Requests;
using MediatR;

namespace Energy.Application.Contracts.ContractParty.Commands.CreateContractParty;

/// <summary>Yeni ContractParty oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateContractPartyCommand(CreateContractPartyRequest Request)
    : IRequest<BaseResponse<Guid>>;
