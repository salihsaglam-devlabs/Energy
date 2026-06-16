using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.CollectionAllocation.Commands.DeleteCollectionAllocation;

/// <summary>CollectionAllocation kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteCollectionAllocationCommand(Guid Id) : IRequest<BaseResponse<bool>>;
