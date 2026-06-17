using FluentValidation;
using Energy.Shared.Models.V1.Organization.LeaveRequest.Requests;

namespace Energy.Application.Organization.LeaveRequest.Validators;

/// <summary>CreateLeaveRequestRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateLeaveRequestRequestValidator : AbstractValidator<CreateLeaveRequestRequest>
{
    public CreateLeaveRequestRequestValidator()
    {
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.LeaveType).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
