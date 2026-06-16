using FluentValidation;
using Energy.Shared.Models.V1.Core.AuditLog.Requests;

namespace Energy.Application.Core.AuditLog.Validators;

/// <summary>UpdateAuditLogRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateAuditLogRequestValidator : AbstractValidator<UpdateAuditLogRequest>
{
    public UpdateAuditLogRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
