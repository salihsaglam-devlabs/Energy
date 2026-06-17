using FluentValidation;
using Energy.Shared.Models.V1.Chat.ChatMessageReaction.Requests;

namespace Energy.Application.Chat.ChatMessageReaction.Validators;

/// <summary>UpdateChatMessageReactionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateChatMessageReactionRequestValidator : AbstractValidator<UpdateChatMessageReactionRequest>
{
    public UpdateChatMessageReactionRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.MessageId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
