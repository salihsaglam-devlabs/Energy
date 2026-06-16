using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using MediatR;

namespace Energy.Application.Chat.Messaging.Queries.GetChatGroupInvites;

/// <summary>GetChatGroupInvites</summary>
public sealed record GetChatGroupInvitesQuery()
    : IRequest<BaseResponse<IReadOnlyList<ChatGroupInviteResponse>>>;
