using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Contracts.Contract.Commands.DeleteContract;

/// <summary>Contract kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteContractCommand(Guid Id) : IRequest<BaseResponse<bool>>;
