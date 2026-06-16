using FluentValidation;
using Energy.Shared.Models.V1.Notifications.NotificationPreference.Requests;

namespace Energy.Application.Modules.Notifications.NotificationPreference.Validators;

/// <summary>CreateNotificationPreferenceRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateNotificationPreferenceRequestValidator : AbstractValidator<CreateNotificationPreferenceRequest>
{
    public CreateNotificationPreferenceRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.NotificationType).NotEmpty();
    }
}
