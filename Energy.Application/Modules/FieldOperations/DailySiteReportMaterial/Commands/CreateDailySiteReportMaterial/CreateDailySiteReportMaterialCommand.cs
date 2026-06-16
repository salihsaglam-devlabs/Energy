using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportMaterial.Requests;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.DailySiteReportMaterial.Commands.CreateDailySiteReportMaterial;

/// <summary>Yeni DailySiteReportMaterial oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateDailySiteReportMaterialCommand(CreateDailySiteReportMaterialRequest Request)
    : IRequest<BaseResponse<Guid>>;
