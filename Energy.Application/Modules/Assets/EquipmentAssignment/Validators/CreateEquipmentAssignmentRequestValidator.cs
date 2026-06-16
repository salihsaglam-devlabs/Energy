using FluentValidation;
using Energy.Shared.Models.V1.Assets.EquipmentAssignment.Requests;

namespace Energy.Application.Modules.Assets.EquipmentAssignment.Validators;

/// <summary>CreateEquipmentAssignmentRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateEquipmentAssignmentRequestValidator : AbstractValidator<CreateEquipmentAssignmentRequest>
{
    public CreateEquipmentAssignmentRequestValidator()
    {
        RuleFor(x => x.EquipmentAssetId).NotEmpty();
    }
}
