using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderStatusHistory.Requests;
using MediatR;

namespace Energy.Application.Operations.WorkOrderStatusHistory.Commands.CreateWorkOrderStatusHistory;

/// <summary>Yeni WorkOrderStatusHistory oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateWorkOrderStatusHistoryCommand(CreateWorkOrderStatusHistoryRequest Request)
    : IRequest<BaseResponse<Guid>>;
