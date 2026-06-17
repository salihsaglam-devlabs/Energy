using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Contracts.ContractAmendment.Commands.DeleteContractAmendment;

/// <summary>ContractAmendment kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteContractAmendmentCommand(Guid Id) : IRequest<BaseResponse<bool>>;
