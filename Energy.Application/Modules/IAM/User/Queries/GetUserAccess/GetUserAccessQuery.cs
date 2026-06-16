using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Modules.IAM.User.Queries.GetUserAccess;

/// <summary>GetUserAccess</summary>
public sealed record GetUserAccessQuery(Guid Id)
    : IRequest<BaseResponse<UserAccessResponse>>;
