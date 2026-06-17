using FluentValidation;
using Energy.Shared.Models.V1.Operations.WorkOrderStatusHistory.Requests;

namespace Energy.Application.Operations.WorkOrderStatusHistory.Validators;

/// <summary>UpdateWorkOrderStatusHistoryRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateWorkOrderStatusHistoryRequestValidator : AbstractValidator<UpdateWorkOrderStatusHistoryRequest>
{
    public UpdateWorkOrderStatusHistoryRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WorkOrderId).NotEmpty();
        RuleFor(x => x.FromStatus).NotEmpty();
        RuleFor(x => x.ToStatus).NotEmpty();
    }
}
