using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using MediatR;

namespace Energy.Application.Chat.Messaging.Queries.GetChatUnreadCount;

/// <summary>GetChatUnreadCount</summary>
public sealed record GetChatUnreadCountQuery()
    : IRequest<BaseResponse<int>>;
