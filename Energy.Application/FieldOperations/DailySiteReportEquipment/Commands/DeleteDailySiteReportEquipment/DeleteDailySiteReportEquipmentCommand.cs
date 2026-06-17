using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.DailySiteReportEquipment.Commands.DeleteDailySiteReportEquipment;

/// <summary>DailySiteReportEquipment kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteDailySiteReportEquipmentCommand(Guid Id) : IRequest<BaseResponse<bool>>;
