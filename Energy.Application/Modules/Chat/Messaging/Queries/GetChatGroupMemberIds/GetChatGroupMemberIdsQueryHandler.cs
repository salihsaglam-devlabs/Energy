using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using Energy.Application.Chat.Services;
using MediatR;

namespace Energy.Application.Modules.Chat.Messaging.Queries.GetChatGroupMemberIds;

/// <summary><see cref="GetChatGroupMemberIdsQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetChatGroupMemberIdsQueryHandler
    : IRequestHandler<GetChatGroupMemberIdsQuery, BaseResponse<IReadOnlyList<Guid>>>
{
    private readonly IChatService _chat;

    public GetChatGroupMemberIdsQueryHandler(IChatService chat)
    {
        _chat = chat;
    }

    public async Task<BaseResponse<IReadOnlyList<Guid>>> Handle(GetChatGroupMemberIdsQuery request, CancellationToken ct)
    {
        var result = await _chat.GetGroupMemberIdsAsync(request.GroupId, ct);
        return BaseResponse<IReadOnlyList<Guid>>.Success(result);
    }
}
