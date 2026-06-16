using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrder.Requests;
using MediatR;

namespace Energy.Application.Operations.WorkOrder.Commands.CreateWorkOrder;

/// <summary>Yeni WorkOrder oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateWorkOrderCommand(CreateWorkOrderRequest Request)
    : IRequest<BaseResponse<Guid>>;
