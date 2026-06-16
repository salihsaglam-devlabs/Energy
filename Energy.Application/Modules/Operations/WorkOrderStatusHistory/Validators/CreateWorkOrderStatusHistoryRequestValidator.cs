using FluentValidation;
using Energy.Shared.Models.V1.Operations.WorkOrderStatusHistory.Requests;

namespace Energy.Application.Modules.Operations.WorkOrderStatusHistory.Validators;

/// <summary>CreateWorkOrderStatusHistoryRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateWorkOrderStatusHistoryRequestValidator : AbstractValidator<CreateWorkOrderStatusHistoryRequest>
{
    public CreateWorkOrderStatusHistoryRequestValidator()
    {
        RuleFor(x => x.WorkOrderId).NotEmpty();
        RuleFor(x => x.FromStatus).NotEmpty();
        RuleFor(x => x.ToStatus).NotEmpty();
    }
}
