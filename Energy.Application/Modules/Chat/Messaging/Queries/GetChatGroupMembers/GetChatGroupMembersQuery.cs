using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using MediatR;

namespace Energy.Application.Modules.Chat.Messaging.Queries.GetChatGroupMembers;

/// <summary>GetChatGroupMembers</summary>
public sealed record GetChatGroupMembersQuery(Guid GroupId)
    : IRequest<BaseResponse<IReadOnlyList<ChatGroupMemberResponse>>>;
