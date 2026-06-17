using FluentValidation;
using Energy.Shared.Models.V1.Assets.EquipmentAssignment.Requests;

namespace Energy.Application.Assets.EquipmentAssignment.Validators;

/// <summary>UpdateEquipmentAssignmentRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateEquipmentAssignmentRequestValidator : AbstractValidator<UpdateEquipmentAssignmentRequest>
{
    public UpdateEquipmentAssignmentRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.EquipmentAssetId).NotEmpty();
    }
}
