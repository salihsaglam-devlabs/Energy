using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.CollectionAllocation.Requests;
using MediatR;

namespace Energy.Application.Finance.CollectionAllocation.Commands.CreateCollectionAllocation;

/// <summary>Yeni CollectionAllocation oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateCollectionAllocationCommand(CreateCollectionAllocationRequest Request)
    : IRequest<BaseResponse<Guid>>;
