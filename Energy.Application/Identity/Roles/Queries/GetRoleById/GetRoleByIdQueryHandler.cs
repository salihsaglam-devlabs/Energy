using Energy.Application.Identity.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Roles.Queries.GetRoleById;

public sealed class GetRoleByIdQueryHandler
    : IRequestHandler<GetRoleByIdQuery, BaseResponse<RoleDetailResponse>>
{
    private readonly IRoleService _roleService;

    public GetRoleByIdQueryHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<BaseResponse<RoleDetailResponse>> Handle(
        GetRoleByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _roleService.GetRoleByIdAsync(request.Id, cancellationToken);
        return BaseResponse<RoleDetailResponse>.Success(result);
    }
}
