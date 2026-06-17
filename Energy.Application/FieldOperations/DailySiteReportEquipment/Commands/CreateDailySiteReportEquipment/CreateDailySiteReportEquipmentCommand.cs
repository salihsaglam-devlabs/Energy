using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportEquipment.Requests;
using MediatR;

namespace Energy.Application.FieldOperations.DailySiteReportEquipment.Commands.CreateDailySiteReportEquipment;

/// <summary>Yeni DailySiteReportEquipment oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateDailySiteReportEquipmentCommand(CreateDailySiteReportEquipmentRequest Request)
    : IRequest<BaseResponse<Guid>>;
