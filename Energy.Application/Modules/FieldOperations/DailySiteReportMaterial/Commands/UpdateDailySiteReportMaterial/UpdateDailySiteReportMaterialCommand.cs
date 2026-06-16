using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportMaterial.Requests;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.DailySiteReportMaterial.Commands.UpdateDailySiteReportMaterial;

/// <summary>Var olan DailySiteReportMaterial kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateDailySiteReportMaterialCommand(Guid Id, UpdateDailySiteReportMaterialRequest Request)
    : IRequest<BaseResponse<bool>>;
