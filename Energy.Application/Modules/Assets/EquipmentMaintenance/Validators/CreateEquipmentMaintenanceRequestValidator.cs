using FluentValidation;
using Energy.Shared.Models.V1.Assets.EquipmentMaintenance.Requests;

namespace Energy.Application.Modules.Assets.EquipmentMaintenance.Validators;

/// <summary>CreateEquipmentMaintenanceRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateEquipmentMaintenanceRequestValidator : AbstractValidator<CreateEquipmentMaintenanceRequest>
{
    public CreateEquipmentMaintenanceRequestValidator()
    {
        RuleFor(x => x.EquipmentAssetId).NotEmpty();
        RuleFor(x => x.MaintenanceType).NotEmpty();
    }
}
