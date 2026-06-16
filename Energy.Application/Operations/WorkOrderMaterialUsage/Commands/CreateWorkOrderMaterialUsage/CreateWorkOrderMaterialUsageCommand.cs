using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialUsage.Requests;
using MediatR;

namespace Energy.Application.Operations.WorkOrderMaterialUsage.Commands.CreateWorkOrderMaterialUsage;

/// <summary>Yeni WorkOrderMaterialUsage oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateWorkOrderMaterialUsageCommand(CreateWorkOrderMaterialUsageRequest Request)
    : IRequest<BaseResponse<Guid>>;
