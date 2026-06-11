using Asp.Versioning;
using Energy.Application.Identity.Services;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/roles")]
public sealed class RolesController : ControllerBase
{
    private readonly IRoleService _roles;
    public RolesController(IRoleService roles) { _roles = roles; }

    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<RoleSummaryResponse>>>> GetAll(
        [FromQuery] PaginatedRequest request, CancellationToken ct)
        => Ok(BaseResponse<PaginatedResponse<RoleSummaryResponse>>.Success(await _roles.GetAllAsync(request, ct)));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<RoleDetailResponse>>> GetById(Guid id, CancellationToken ct)
    {
        var role = await _roles.GetByIdAsync(id, ct);
        return role is null ? NotFound(BaseResponse<RoleDetailResponse>.Failure("Role not found."))
                            : Ok(BaseResponse<RoleDetailResponse>.Success(role));
    }

    [HttpPost]
    public async Task<ActionResult<BaseResponse<RoleDetailResponse>>> Create(CreateRoleRequest request, CancellationToken ct)
        => Ok(BaseResponse<RoleDetailResponse>.Success(await _roles.CreateAsync(request, ct)));

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<RoleDetailResponse>>> Update(Guid id, UpdateRoleRequest request, CancellationToken ct)
        => Ok(BaseResponse<RoleDetailResponse>.Success(await _roles.UpdateAsync(id, request, ct)));

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(BaseResponse<bool>.Success(await _roles.DeleteAsync(id, ct)));

    [HttpPut("{id:guid}/permissions")]
    public async Task<ActionResult<BaseResponse<RoleDetailResponse>>> SetPermissions(
        Guid id, SetRolePermissionsRequest request, CancellationToken ct)
        => Ok(BaseResponse<RoleDetailResponse>.Success(await _roles.SetPermissionsAsync(id, request, ct)));
}
