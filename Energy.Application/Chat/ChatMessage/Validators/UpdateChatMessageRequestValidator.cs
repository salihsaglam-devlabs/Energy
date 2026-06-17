using FluentValidation;
using Energy.Shared.Models.V1.Chat.ChatMessage.Requests;

namespace Energy.Application.Chat.ChatMessage.Validators;

/// <summary>UpdateChatMessageRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateChatMessageRequestValidator : AbstractValidator<UpdateChatMessageRequest>
{
    public UpdateChatMessageRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.SenderId).NotEmpty();
    }
}
