using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using MediatR;

namespace Energy.Application.Chat.Messaging.Queries.GetChatConversation;

/// <summary>GetChatConversation</summary>
public sealed record GetChatConversationQuery(Guid PeerId)
    : IRequest<BaseResponse<IReadOnlyList<ChatMessageResponse>>>;
