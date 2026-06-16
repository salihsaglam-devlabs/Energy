using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractLine.Requests;
using MediatR;

namespace Energy.Application.Contracts.ContractLine.Commands.UpdateContractLine;

/// <summary>Var olan ContractLine kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateContractLineCommand(Guid Id, UpdateContractLineRequest Request)
    : IRequest<BaseResponse<bool>>;
