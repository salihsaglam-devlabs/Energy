using FluentValidation;
using Energy.Shared.Models.V1.Chat.ChatGroup.Requests;

namespace Energy.Application.Chat.ChatGroup.Validators;

/// <summary>CreateChatGroupRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateChatGroupRequestValidator : AbstractValidator<CreateChatGroupRequest>
{
    public CreateChatGroupRequestValidator()
    {
        RuleFor(x => x.OwnerId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
