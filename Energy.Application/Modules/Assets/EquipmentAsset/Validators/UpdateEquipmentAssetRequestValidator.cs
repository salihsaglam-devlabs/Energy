using FluentValidation;
using Energy.Shared.Models.V1.Assets.EquipmentAsset.Requests;

namespace Energy.Application.Modules.Assets.EquipmentAsset.Validators;

/// <summary>UpdateEquipmentAssetRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateEquipmentAssetRequestValidator : AbstractValidator<UpdateEquipmentAssetRequest>
{
    public UpdateEquipmentAssetRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.CompanyId).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.AssetType).NotEmpty();
    }
}
