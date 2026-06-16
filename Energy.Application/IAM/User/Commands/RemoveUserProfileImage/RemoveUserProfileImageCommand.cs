using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.IAM.User.Commands.RemoveUserProfileImage;

/// <summary>RemoveUserProfileImage</summary>
public sealed record RemoveUserProfileImageCommand(Guid Id)
    : IRequest<BaseResponse<bool>>;
