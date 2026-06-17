using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderMaterialUsage.Commands.DeleteWorkOrderMaterialUsage;

/// <summary>WorkOrderMaterialUsage kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteWorkOrderMaterialUsageCommand(Guid Id) : IRequest<BaseResponse<bool>>;
