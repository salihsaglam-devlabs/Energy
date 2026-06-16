using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentMaintenance.Requests;
using MediatR;

namespace Energy.Application.Modules.Assets.EquipmentMaintenance.Commands.CreateEquipmentMaintenance;

/// <summary>Yeni EquipmentMaintenance oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateEquipmentMaintenanceCommand(CreateEquipmentMaintenanceRequest Request)
    : IRequest<BaseResponse<Guid>>;
