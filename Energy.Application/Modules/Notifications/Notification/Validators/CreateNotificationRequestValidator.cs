using FluentValidation;
using Energy.Shared.Models.V1.Notifications.Notification.Requests;

namespace Energy.Application.Modules.Notifications.Notification.Validators;

/// <summary>CreateNotificationRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateNotificationRequestValidator : AbstractValidator<CreateNotificationRequest>
{
    public CreateNotificationRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.NotificationType).NotEmpty();
    }
}
