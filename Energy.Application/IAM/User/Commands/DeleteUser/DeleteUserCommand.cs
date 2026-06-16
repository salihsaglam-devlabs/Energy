using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.IAM.User.Commands.DeleteUser;

/// <summary>DeleteUser</summary>
public sealed record DeleteUserCommand(Guid Id)
    : IRequest<BaseResponse<bool>>;
