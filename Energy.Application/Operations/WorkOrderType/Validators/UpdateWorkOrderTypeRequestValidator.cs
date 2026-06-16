using FluentValidation;
using Energy.Shared.Models.V1.Operations.WorkOrderType.Requests;

namespace Energy.Application.Operations.WorkOrderType.Validators;

/// <summary>UpdateWorkOrderTypeRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateWorkOrderTypeRequestValidator : AbstractValidator<UpdateWorkOrderTypeRequest>
{
    public UpdateWorkOrderTypeRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
