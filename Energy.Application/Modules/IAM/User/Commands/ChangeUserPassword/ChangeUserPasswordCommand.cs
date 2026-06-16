using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Modules.IAM.User.Commands.ChangeUserPassword;

/// <summary>ChangeUserPassword</summary>
public sealed record ChangeUserPasswordCommand(Guid Id, ChangePasswordRequest Request)
    : IRequest<BaseResponse<bool>>;
