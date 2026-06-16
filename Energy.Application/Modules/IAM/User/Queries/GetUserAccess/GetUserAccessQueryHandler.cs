using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.Modules.IAM.User.Queries.GetUserAccess;

/// <summary><see cref="GetUserAccessQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetUserAccessQueryHandler
    : IRequestHandler<GetUserAccessQuery, BaseResponse<UserAccessResponse>>
{
    private readonly IUserService _users;

    public GetUserAccessQueryHandler(IUserService users)
    {
        _users = users;
    }

    public async Task<BaseResponse<UserAccessResponse>> Handle(GetUserAccessQuery request, CancellationToken ct)
    {
        var result = await _users.GetAccessAsync(request.Id, ct)
            ?? throw new NotFoundException(LocalizationKeys.Messages.UserNotFound, request.Id);
        return BaseResponse<UserAccessResponse>.Success(result);
    }
}
