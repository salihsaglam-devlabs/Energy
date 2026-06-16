using FluentValidation;
using Energy.Shared.Models.V1.Operations.WorkOrderType.Requests;

namespace Energy.Application.Operations.WorkOrderType.Validators;

/// <summary>CreateWorkOrderTypeRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateWorkOrderTypeRequestValidator : AbstractValidator<CreateWorkOrderTypeRequest>
{
    public CreateWorkOrderTypeRequestValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
