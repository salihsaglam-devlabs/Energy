using FluentValidation;
using Energy.Shared.Models.V1.Notifications.Notification.Requests;

namespace Energy.Application.Modules.Notifications.Notification.Validators;

/// <summary>UpdateNotificationRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateNotificationRequestValidator : AbstractValidator<UpdateNotificationRequest>
{
    public UpdateNotificationRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.NotificationType).NotEmpty();
    }
}
