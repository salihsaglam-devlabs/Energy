using FluentValidation;
using Energy.Shared.Models.V1.Assets.EquipmentMaintenance.Requests;

namespace Energy.Application.Assets.EquipmentMaintenance.Validators;

/// <summary>UpdateEquipmentMaintenanceRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateEquipmentMaintenanceRequestValidator : AbstractValidator<UpdateEquipmentMaintenanceRequest>
{
    public UpdateEquipmentMaintenanceRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.EquipmentAssetId).NotEmpty();
        RuleFor(x => x.MaintenanceType).NotEmpty();
    }
}
