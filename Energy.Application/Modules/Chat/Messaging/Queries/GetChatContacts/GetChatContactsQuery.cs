using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using MediatR;

namespace Energy.Application.Modules.Chat.Messaging.Queries.GetChatContacts;

/// <summary>GetChatContacts</summary>
public sealed record GetChatContactsQuery()
    : IRequest<BaseResponse<IReadOnlyList<ChatContactResponse>>>;
