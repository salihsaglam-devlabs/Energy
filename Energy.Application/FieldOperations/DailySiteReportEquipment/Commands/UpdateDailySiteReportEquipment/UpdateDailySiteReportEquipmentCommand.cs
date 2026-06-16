using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportEquipment.Requests;
using MediatR;

namespace Energy.Application.FieldOperations.DailySiteReportEquipment.Commands.UpdateDailySiteReportEquipment;

/// <summary>Var olan DailySiteReportEquipment kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateDailySiteReportEquipmentCommand(Guid Id, UpdateDailySiteReportEquipmentRequest Request)
    : IRequest<BaseResponse<bool>>;
