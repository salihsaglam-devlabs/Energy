using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using MediatR;

namespace Energy.Application.Modules.Chat.Messaging.Queries.GetChatAttachment;

/// <summary>GetChatAttachment</summary>
public sealed record GetChatAttachmentQuery(Guid MessageId)
    : IRequest<ChatAttachmentResponse?>;
