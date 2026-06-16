using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using MediatR;

namespace Energy.Application.Modules.Chat.Messaging.Commands.MarkChatRead;

/// <summary>MarkChatRead</summary>
public sealed record MarkChatReadCommand(Guid PeerId)
    : IRequest<BaseResponse<int>>;
