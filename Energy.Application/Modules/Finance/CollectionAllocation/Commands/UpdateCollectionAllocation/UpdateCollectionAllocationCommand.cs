using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.CollectionAllocation.Requests;
using MediatR;

namespace Energy.Application.Modules.Finance.CollectionAllocation.Commands.UpdateCollectionAllocation;

/// <summary>Var olan CollectionAllocation kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateCollectionAllocationCommand(Guid Id, UpdateCollectionAllocationRequest Request)
    : IRequest<BaseResponse<bool>>;
