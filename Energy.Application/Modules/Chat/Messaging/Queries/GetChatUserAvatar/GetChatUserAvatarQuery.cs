using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using MediatR;

namespace Energy.Application.Modules.Chat.Messaging.Queries.GetChatUserAvatar;

/// <summary>GetChatUserAvatar</summary>
public sealed record GetChatUserAvatarQuery(Guid UserId)
    : IRequest<ChatAttachmentResponse?>;
