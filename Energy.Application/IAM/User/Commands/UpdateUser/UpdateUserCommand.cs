using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.IAM.User.Commands.UpdateUser;

/// <summary>UpdateUser</summary>
public sealed record UpdateUserCommand(Guid Id, UpdateUserRequest Request)
    : IRequest<BaseResponse<UserDetailResponse>>;
