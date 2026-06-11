using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Users.Commands.UpdateProfileImage;

public sealed record UpdateProfileImageCommand(Guid UserId, byte[] Content, string ContentType)
    : IRequest<BaseResponse<UserDetailResponse>>;

