using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Modules.IAM.User.Queries.GetUserById;

/// <summary>GetUserById</summary>
public sealed record GetUserByIdQuery(Guid Id)
    : IRequest<BaseResponse<UserDetailResponse>>;
