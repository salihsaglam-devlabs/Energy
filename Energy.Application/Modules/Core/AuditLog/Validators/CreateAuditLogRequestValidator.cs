using FluentValidation;
using Energy.Shared.Models.V1.Core.AuditLog.Requests;

namespace Energy.Application.Modules.Core.AuditLog.Validators;

/// <summary>CreateAuditLogRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateAuditLogRequestValidator : AbstractValidator<CreateAuditLogRequest>
{
    public CreateAuditLogRequestValidator()
    {
        // Zorunlu iş alanı yok; yapısal doğrulama için yer tutucu.
    }
}
