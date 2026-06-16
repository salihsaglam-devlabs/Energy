using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.Modules.IAM.User.Queries.GetUserById;

/// <summary><see cref="GetUserByIdQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetUserByIdQueryHandler
    : IRequestHandler<GetUserByIdQuery, BaseResponse<UserDetailResponse>>
{
    private readonly IUserService _users;

    public GetUserByIdQueryHandler(IUserService users)
    {
        _users = users;
    }

    public async Task<BaseResponse<UserDetailResponse>> Handle(GetUserByIdQuery request, CancellationToken ct)
    {
        var result = await _users.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(LocalizationKeys.Messages.UserNotFound, request.Id);
        return BaseResponse<UserDetailResponse>.Success(result);
    }
}
