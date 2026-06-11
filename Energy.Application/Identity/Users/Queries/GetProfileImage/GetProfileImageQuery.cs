using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Users.Queries.GetProfileImage;

public sealed record GetProfileImageQuery(Guid UserId)
    : IRequest<BaseResponse<ProfileImageResponse?>>;

