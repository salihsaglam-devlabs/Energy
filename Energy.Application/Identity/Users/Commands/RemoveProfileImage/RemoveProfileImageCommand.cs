using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Users.Commands.RemoveProfileImage;

public sealed record RemoveProfileImageCommand(Guid UserId)
    : IRequest<BaseResponse<UserDetailResponse>>;

