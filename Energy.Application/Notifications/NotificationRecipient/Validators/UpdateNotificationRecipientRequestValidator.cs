using FluentValidation;
using Energy.Shared.Models.V1.Notifications.NotificationRecipient.Requests;

namespace Energy.Application.Notifications.NotificationRecipient.Validators;

/// <summary>UpdateNotificationRecipientRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateNotificationRecipientRequestValidator : AbstractValidator<UpdateNotificationRecipientRequest>
{
    public UpdateNotificationRecipientRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.NotificationId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
