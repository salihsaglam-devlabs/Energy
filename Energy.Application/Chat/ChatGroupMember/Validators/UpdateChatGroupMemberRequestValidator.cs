using FluentValidation;
using Energy.Shared.Models.V1.Chat.ChatGroupMember.Requests;

namespace Energy.Application.Chat.ChatGroupMember.Validators;

/// <summary>UpdateChatGroupMemberRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateChatGroupMemberRequestValidator : AbstractValidator<UpdateChatGroupMemberRequest>
{
    public UpdateChatGroupMemberRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.GroupId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
