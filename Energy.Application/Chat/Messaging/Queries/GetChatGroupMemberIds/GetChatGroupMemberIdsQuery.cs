using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using MediatR;

namespace Energy.Application.Chat.Messaging.Queries.GetChatGroupMemberIds;

/// <summary>GetChatGroupMemberIds</summary>
public sealed record GetChatGroupMemberIdsQuery(Guid GroupId)
    : IRequest<BaseResponse<IReadOnlyList<Guid>>>;
