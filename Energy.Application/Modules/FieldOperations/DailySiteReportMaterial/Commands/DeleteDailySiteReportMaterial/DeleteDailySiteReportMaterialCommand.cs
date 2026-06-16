using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.DailySiteReportMaterial.Commands.DeleteDailySiteReportMaterial;

/// <summary>DailySiteReportMaterial kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteDailySiteReportMaterialCommand(Guid Id) : IRequest<BaseResponse<bool>>;
