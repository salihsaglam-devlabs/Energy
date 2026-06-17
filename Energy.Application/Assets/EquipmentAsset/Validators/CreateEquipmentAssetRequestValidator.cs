using FluentValidation;
using Energy.Shared.Models.V1.Assets.EquipmentAsset.Requests;

namespace Energy.Application.Assets.EquipmentAsset.Validators;

/// <summary>CreateEquipmentAssetRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateEquipmentAssetRequestValidator : AbstractValidator<CreateEquipmentAssetRequest>
{
    public CreateEquipmentAssetRequestValidator()
    {
        RuleFor(x => x.CompanyId).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.AssetType).NotEmpty();
    }
}
