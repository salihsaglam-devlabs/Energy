using FluentValidation;
using Energy.Shared.Models.V1.Notifications.NotificationRecipient.Requests;

namespace Energy.Application.Notifications.NotificationRecipient.Validators;

/// <summary>CreateNotificationRecipientRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateNotificationRecipientRequestValidator : AbstractValidator<CreateNotificationRecipientRequest>
{
    public CreateNotificationRecipientRequestValidator()
    {
        RuleFor(x => x.NotificationId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
