using FluentValidation;
using Energy.Shared.Models.V1.Chat.ChatMessage.Requests;

namespace Energy.Application.Modules.Chat.ChatMessage.Validators;

/// <summary>CreateChatMessageRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateChatMessageRequestValidator : AbstractValidator<CreateChatMessageRequest>
{
    public CreateChatMessageRequestValidator()
    {
        RuleFor(x => x.SenderId).NotEmpty();
    }
}
