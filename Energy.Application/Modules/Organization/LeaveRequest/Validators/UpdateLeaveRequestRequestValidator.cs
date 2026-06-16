using FluentValidation;
using Energy.Shared.Models.V1.Organization.LeaveRequest.Requests;

namespace Energy.Application.Modules.Organization.LeaveRequest.Validators;

/// <summary>UpdateLeaveRequestRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateLeaveRequestRequestValidator : AbstractValidator<UpdateLeaveRequestRequest>
{
    public UpdateLeaveRequestRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.LeaveType).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
