using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.Contract.Requests;
using MediatR;

namespace Energy.Application.Modules.Contracts.Contract.Commands.UpdateContract;

/// <summary>Var olan Contract kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateContractCommand(Guid Id, UpdateContractRequest Request)
    : IRequest<BaseResponse<bool>>;
