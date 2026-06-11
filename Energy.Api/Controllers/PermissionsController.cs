using Asp.Versioning;
using Energy.Application.Identity.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/permissions")]
public sealed class PermissionsController : ControllerBase
{
    private readonly IPermissionService _permissions;
    public PermissionsController(IPermissionService permissions) { _permissions = permissions; }

    [HttpGet]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<PermissionResponse>>>> GetAll(CancellationToken ct)
        => Ok(BaseResponse<IReadOnlyList<PermissionResponse>>.Success(await _permissions.GetAllAsync(ct)));

    [HttpGet("{code}")]
    public async Task<ActionResult<BaseResponse<PermissionResponse>>> GetByCode(string code, CancellationToken ct)
    {
        var item = await _permissions.GetByCodeAsync(code, ct);
        return item is null ? NotFound(BaseResponse<PermissionResponse>.Failure("Permission not found."))
                            : Ok(BaseResponse<PermissionResponse>.Success(item));
    }
}
