using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Modules.IAM.User.Queries.GetUserProfileImage;

/// <summary>GetUserProfileImage</summary>
public sealed record GetUserProfileImageQuery(Guid Id)
    : IRequest<ProfileImageResponse?>;
