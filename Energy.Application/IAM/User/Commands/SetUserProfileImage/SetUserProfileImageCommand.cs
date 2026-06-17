using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.IAM.User.Commands.SetUserProfileImage;

/// <summary>SetUserProfileImage</summary>
public sealed record SetUserProfileImageCommand(Guid Id, SetProfileImageRequest Request)
    : IRequest<BaseResponse<bool>>;
