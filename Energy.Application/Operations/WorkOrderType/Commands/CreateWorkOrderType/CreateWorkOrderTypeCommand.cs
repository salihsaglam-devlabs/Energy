using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderType.Requests;
using MediatR;

namespace Energy.Application.Operations.WorkOrderType.Commands.CreateWorkOrderType;

/// <summary>Yeni WorkOrderType oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateWorkOrderTypeCommand(CreateWorkOrderTypeRequest Request)
    : IRequest<BaseResponse<Guid>>;
