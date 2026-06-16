using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractAmendment.Requests;
using MediatR;

namespace Energy.Application.Modules.Contracts.ContractAmendment.Commands.UpdateContractAmendment;

/// <summary>Var olan ContractAmendment kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateContractAmendmentCommand(Guid Id, UpdateContractAmendmentRequest Request)
    : IRequest<BaseResponse<bool>>;
