using FluentValidation;
using Energy.Shared.Models.V1.Chat.ChatGroupMember.Requests;

namespace Energy.Application.Modules.Chat.ChatGroupMember.Validators;

/// <summary>CreateChatGroupMemberRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateChatGroupMemberRequestValidator : AbstractValidator<CreateChatGroupMemberRequest>
{
    public CreateChatGroupMemberRequestValidator()
    {
        RuleFor(x => x.GroupId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
