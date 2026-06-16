using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Modules.IAM.User.Commands.SetUserAccess;

/// <summary>SetUserAccess</summary>
public sealed record SetUserAccessCommand(Guid Id, SetUserAccessRequest Request)
    : IRequest<BaseResponse<UserAccessResponse>>;
