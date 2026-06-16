using FluentValidation;
using Energy.Shared.Models.V1.Chat.ChatMessageReaction.Requests;

namespace Energy.Application.Modules.Chat.ChatMessageReaction.Validators;

/// <summary>CreateChatMessageReactionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateChatMessageReactionRequestValidator : AbstractValidator<CreateChatMessageReactionRequest>
{
    public CreateChatMessageReactionRequestValidator()
    {
        RuleFor(x => x.MessageId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
