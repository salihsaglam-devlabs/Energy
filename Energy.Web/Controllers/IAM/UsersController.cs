using System.Linq;
using Energy.Shared.Identity.Permissions;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Web.Clients.Identity;
using Energy.Web.Common.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Web.Controllers.IAM;

/// <summary>
/// Kullanıcılar sayfası ince bir DevExtreme dxDataGrid kabuğudur. Index aksiyonu boş
/// görünümü döndürür; ızgara verisini, aşağıdaki aksiyon metotlarının sunduğu JSON
/// uç noktalarından çeker. Tüm istekler API'ye vekillenir.
/// </summary>
[Authorize]
[PagePermission(PermissionCatalog.UserReadAll)]
[Route("users")]
public sealed class UsersController : Controller
{
    private readonly IUserApiClient _users;
    private readonly IRoleApiClient _roles;

    public UsersController(IUserApiClient users, IRoleApiClient roles)
    {
        _users = users;
        _roles = roles;
    }

    [HttpGet("")]
    [HttpGet("index")]
    public IActionResult Index() => View();

    /// <summary>DevExtreme CustomStore load endpoint.</summary>
    [HttpGet("list")]
    public async Task<IActionResult> List(int skip = 0, int take = 20, string? searchValue = null, CancellationToken ct = default)
    {
        var pageNumber = (take <= 0 ? 1 : skip / take) + 1;
        var pageSize = take <= 0 ? 20 : take;

        var envelope = await _users.GetAllAsync(new PaginatedRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            Search = string.IsNullOrWhiteSpace(searchValue) ? null : searchValue
        }, ct);

        var page = envelope.Data;
        var items = (page?.Items ?? Array.Empty<Shared.Models.V1.Identity.Responses.UserSummaryResponse>())
            .Select(u =>
            {
                var fullParts = (u.FullName ?? string.Empty).Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                return new
                {
                    id = u.Id,
                    userName = u.UserName,
                    email = u.Email,
                    firstName = fullParts.Length > 0 ? fullParts[0] : string.Empty,
                    lastName = fullParts.Length > 1 ? fullParts[1] : string.Empty,
                    fullName = u.FullName,
                    isActive = u.IsActive,
                    lastLoginAt = u.LastLoginAt,
                    roleNames = u.RoleNames
                };
            })
            .ToArray();

        return Json(new { data = items, totalCount = page?.TotalCount ?? 0 });
    }

    /// <summary>Roller etiket kutusu için arama (lookup) beslemesi.</summary>
    [HttpGet("roles-lookup")]
    public async Task<IActionResult> RolesLookup(CancellationToken ct)
    {
        var envelope = await _roles.GetAllAsync(new PaginatedRequest { PageNumber = 1, PageSize = 200 }, ct);
        var items = (envelope.Data?.Items ?? Array.Empty<Shared.Models.V1.Identity.Responses.RoleSummaryResponse>())
            .Select(r => new { id = r.Id, name = r.Name })
            .ToArray();
        return Json(items);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var envelope = await _users.GetByIdAsync(id, ct);
        if (envelope.Data is null) return NotFound();
        var u = envelope.Data;
        return Json(new
        {
            data = new
            {
                id = u.Id,
                userName = u.UserName,
                email = u.Email,
                firstName = u.FirstName,
                lastName = u.LastName,
                phoneNumber = (string?)null,
                isActive = u.IsActive,
                emailConfirmed = false,
                phoneNumberConfirmed = false,
                twoFactorEnabled = false,
                lockoutEnabled = false,
                roles = u.Roles.Select(r => new { id = r.Id, name = r.Name, description = r.Description })
            }
        });
    }

    public sealed class UserCreateInput
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public List<Guid> RoleIds { get; set; } = new();
    }

    [HttpPost("")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Create([FromBody] UserCreateInput input, CancellationToken ct)
    {
        var envelope = await _users.CreateAsync(new CreateUserRequest
        {
            UserName = input.UserName,
            Email = input.Email,
            FirstName = input.FirstName,
            LastName = input.LastName,
            Password = input.Password,
            IsActive = input.IsActive,
            RoleIds = input.RoleIds
        }, ct);
        return Json(envelope);
    }

    public sealed class UserUpdateInput
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }

    [HttpPut("{id:guid}")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Update(Guid id, [FromBody] UserUpdateInput input, CancellationToken ct)
    {
        // Sayfa içi düzenleme formu rolleri göstermediğinden kullanıcının mevcut rol
        // atamalarını koru (bundan özel "Rolleri yönet" açılır penceresi sorumludur).
        var existing = (await _users.GetByIdAsync(id, ct)).Data;
        var roleIds = existing?.Roles.Select(r => r.Id).ToArray() ?? Array.Empty<Guid>();

        var envelope = await _users.UpdateAsync(id, new UpdateUserRequest
        {
            FirstName = input.FirstName,
            LastName = input.LastName,
            Email = input.Email,
            IsActive = input.IsActive,
            RoleIds = roleIds
        }, ct);
        return Json(envelope);
    }

    public sealed class UserRolesInput { public List<Guid> RoleIds { get; set; } = new(); }

    [HttpPut("{id:guid}/roles")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> SetRoles(Guid id, [FromBody] UserRolesInput input, CancellationToken ct)
    {
        var existing = (await _users.GetByIdAsync(id, ct)).Data;
        if (existing is null) return NotFound();

        var envelope = await _users.UpdateAsync(id, new UpdateUserRequest
        {
            FirstName = existing.FirstName,
            LastName = existing.LastName,
            Email = existing.Email,
            IsActive = existing.IsActive,
            RoleIds = input.RoleIds
        }, ct);
        return Json(envelope);
    }

    public sealed class UserPasswordInput { public string NewPassword { get; set; } = string.Empty; }

    [HttpPut("{id:guid}/password")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> ChangePassword(Guid id, [FromBody] UserPasswordInput input, CancellationToken ct)
    {
        var envelope = await _users.ChangePasswordAsync(id, new ChangePasswordRequest
        {
            NewPassword = input.NewPassword
        }, ct);
        return Json(envelope);
    }

    [HttpDelete("{id:guid}")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var envelope = await _users.DeleteAsync(id, ct);
        return Json(envelope);
    }
}
