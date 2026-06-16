using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using MediatR;

namespace Energy.Application.Chat.Messaging.Queries.GetChatGroupConversation;

/// <summary>GetChatGroupConversation</summary>
public sealed record GetChatGroupConversationQuery(Guid GroupId)
    : IRequest<BaseResponse<IReadOnlyList<ChatMessageResponse>>>;
