using FluentValidation;
using Energy.Shared.Models.V1.Chat.ChatGroup.Requests;

namespace Energy.Application.Modules.Chat.ChatGroup.Validators;

/// <summary>UpdateChatGroupRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateChatGroupRequestValidator : AbstractValidator<UpdateChatGroupRequest>
{
    public UpdateChatGroupRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.OwnerId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
