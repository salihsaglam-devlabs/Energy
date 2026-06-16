using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklistItem.Requests;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderChecklistItem.Commands.CreateWorkOrderChecklistItem;

/// <summary>Yeni WorkOrderChecklistItem oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateWorkOrderChecklistItemCommand(CreateWorkOrderChecklistItemRequest Request)
    : IRequest<BaseResponse<Guid>>;
