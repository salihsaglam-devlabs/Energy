using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.IAM.Permission.Queries.GetPermissionByCode;

/// <summary><see cref="GetPermissionByCodeQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetPermissionByCodeQueryHandler
    : IRequestHandler<GetPermissionByCodeQuery, BaseResponse<PermissionResponse>>
{
    private readonly IPermissionService _permissions;

    public GetPermissionByCodeQueryHandler(IPermissionService permissions)
    {
        _permissions = permissions;
    }

    public async Task<BaseResponse<PermissionResponse>> Handle(GetPermissionByCodeQuery request, CancellationToken ct)
    {
        var result = await _permissions.GetByCodeAsync(request.Code, ct);
        return result is null
            ? BaseResponse<PermissionResponse>.Failure("Permission not found.")
            : BaseResponse<PermissionResponse>.Success(result);
    }
}
