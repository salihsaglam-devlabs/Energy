#!/usr/bin/env python3
"""Özel/process/report controller endpoint spec'leri (generate_special_cqrs.py tüketir)."""

# --- Shared model namespace'leri ---
CR = "Energy.Shared.Models.V1.Common.Requests"
IDR = "Energy.Shared.Models.V1.Identity.Requests"
IDP = "Energy.Shared.Models.V1.Identity.Responses"
SYR = "Energy.Shared.Models.V1.System.Requests"
SYP = "Energy.Shared.Models.V1.System.Responses"
LGR = "Energy.Shared.Models.V1.Logger.Requests"
LGP = "Energy.Shared.Models.V1.Logger.Responses"
LCR = "Energy.Shared.Models.V1.Localization.Requests"
LCP = "Energy.Shared.Models.V1.Localization.Responses"
STR = "Energy.Shared.Models.V1.Settings.Requests"
STP = "Energy.Shared.Models.V1.Settings.Responses"
HMR = "Energy.Shared.Models.V1.Home.Requests"
HMP = "Energy.Shared.Models.V1.Home.Responses"
CHR = "Energy.Shared.Models.V1.Chat.Requests"
CHP = "Energy.Shared.Models.V1.Chat.Responses"

# --- inject sabitleri (interface, field, namespace) ---
US = ("IUserService", "_users", "Energy.Application.Identity.Services")
RS = ("IRoleService", "_roles", "Energy.Application.Identity.Services")
PS = ("IPermissionService", "_permissions", "Energy.Application.Identity.Services")
MS = ("IMenuService", "_menus", "Energy.Application.System.Services")
AES = ("IApiEndpointService", "_endpoints", "Energy.Application.System.Services")
CS = ("IChatService", "_chat", "Energy.Application.Chat.Services")
LS = ("ILocalizationService", "_localization", "Energy.Application.Localization.Services")
STS = ("IUserSettingsService", "_settings", "Energy.Application.Settings.Services")
HS = ("IHomeService", "_home", "Energy.Application.Home.Services")
SEED = ("ISystemSeeder", "_seeder", "Energy.Application.System.Services")
ALS = ("IAuditLogService", "_logs", "Energy.Application.Logger.Services")
INV = ("IInventoryService", "_inventory", "Energy.Application.Inventory.Services")
FIN = ("IFinanceService", "_finance", "Energy.Application.Finance.Services")
GR = ("IGoodsReceiptService", "_goodsReceipt", "Energy.Application.Procurement.Services")
LOC = ("IStringLocalizer<SharedResource>", "_localizer", "Microsoft.Extensions.Localization")


def E(feature, name, kind, action, attr, inner, mode, inject, call,
      sig="", new_args="", ctor="", **kw):
    d = dict(feature=feature, name=name, kind=kind, action=action, attr=attr,
             inner=inner, mode=mode, inject=inject, call=call,
             sig=sig, new_args=new_args, ctor=ctor)
    d.update(kw)
    return d


def report(module_path, controller, route, feature, svc_iface, svc_ns, req, row, csv):
    inject = [(svc_iface, "_service", svc_ns)]
    data_name = f"Get{feature.split('/')[-1]}Data"
    data_ns = f"Energy.Application.Modules.{module_path.replace('.Reports', '.Reports')}.{feature.replace('/', '.')}.Queries.{data_name}"
    # module_path for reports is e.g. "Procurement.Reports"; feature "Reports/PurchaseOrderSummary"
    fm = module_path.split(".")[0]
    data_ns = f"Energy.Application.Modules.{fm}.{feature.replace('/', '.')}.Queries.{data_name}"
    export_body = (
        f"        request.PageNumber = 1;\n"
        f"        request.PageSize = 100000;\n"
        f"        var result = await _mediator.Send(new {data_name}Query(request), ct);\n"
        f"        return File(CsvExport.ToBytes(result.Data?.Items), \"text/csv\", \"{csv}\");")
    return {
        "controller": controller,
        "module": module_path,
        "feature_module": fm,
        "route": route,
        "summary": f"{feature.split('/')[-1]} raporu uç noktaları (veri + export). Salt-okunur.",
        "model_usings": [f"Energy.Shared.Models.V1.{module_path}.{feature.split('/')[-1]}.Requests",
                          f"Energy.Shared.Models.V1.{module_path}.{feature.split('/')[-1]}.Responses"],
        "ctrl_usings": [f"Energy.Shared.Models.V1.{module_path}.{feature.split('/')[-1]}.Requests",
                        f"Energy.Shared.Models.V1.{module_path}.{feature.split('/')[-1]}.Responses"],
        "ctrl_extra_usings": ["Energy.Api.Common.Export"],
        "endpoints": [
            E(feature, data_name, "query", "GetData", "[HttpGet]",
              f"PaginatedResponse<{row}>", "passthrough", inject,
              "_service.GetDataAsync(request.Request, ct)",
              sig=f"[FromQuery] {req} request", new_args="request", ctor=f"{req} Request",
              doc=f"{feature.split('/')[-1]} rapor verisi (filtreli, sayfalı)."),
            {"feature": feature, "name": data_name, "kind": "query", "action": "Export",
             "attr": '[HttpGet("export")]', "inner": f"PaginatedResponse<{row}>", "mode": "export",
             "sig": f"[FromQuery] {req} request", "export_body": export_body, "uses_ns": data_ns},
        ],
    }


SPECS = [
    # ===================== Home =====================
    {
        "controller": "HomeController", "module": "Home", "feature_module": "Home",
        "route": "api/v{version:apiVersion}/home",
        "summary": "Ana sayfa/dashboard uç noktaları (kurumsal metrikler).",
        "model_usings": [HMR, HMP], "ctrl_usings": [HMR, HMP],
        "endpoints": [
            E("Dashboard", "GetHomeDashboard", "query", "GetDashboard", '[HttpGet("dashboard")]',
              "HomeDashboardResponse", "wrap", [HS],
              "_home.GetDashboardAsync(request.Request, ct)",
              sig="[FromQuery] GetHomeDashboardRequest request", new_args="request", ctor="GetHomeDashboardRequest Request"),
            E("Dashboard", "GetEnterpriseMetrics", "query", "EnterpriseMetrics", '[HttpGet("enterprise-metrics")]',
              "IReadOnlyList<EnterpriseMetricResponse>", "wrap", [HS],
              "_home.GetEnterpriseMetricsAsync(ct)"),
        ],
    },

    # ===================== IAM: Auth =====================
    {
        "controller": "AuthController", "module": "IAM", "feature_module": "IAM",
        "route": "api/v{version:apiVersion}/auth", "allow_anonymous_any": True,
        "summary": "Kimlik doğrulama uç noktaları (login).",
        "model_usings": [IDR, IDP], "ctrl_usings": [IDR, IDP],
        "endpoints": [
            E("Auth", "Login", "command", "Login", '[HttpPost("login")]',
              "AuthTokenResponse", "auth", [US, LOC],
              "_users.LoginAsync(request.Request, ct)",
              sig="LoginRequest request", new_args="request", ctor="LoginRequest Request",
              allow_anonymous=True),
        ],
    },

    # ===================== IAM: Users =====================
    {
        "controller": "UsersController", "module": "IAM", "feature_module": "IAM",
        "route": "api/v{version:apiVersion}/users",
        "summary": "Kullanıcı yönetimi uç noktaları (IAM).",
        "model_usings": [CR, IDR, IDP], "ctrl_usings": [CR, IDR, IDP],
        "endpoints": [
            E("User", "GetUserList", "query", "GetAll", "[HttpGet]",
              "PaginatedResponse<UserSummaryResponse>", "wrap", [US],
              "_users.GetAllAsync(request.Request, ct)",
              sig="[FromQuery] PaginatedRequest request", new_args="request", ctor="PaginatedRequest Request"),
            E("User", "GetUserById", "query", "GetById", '[HttpGet("{id:guid}")]',
              "UserDetailResponse", "notfound_throw", [US],
              "_users.GetByIdAsync(request.Id, ct)",
              sig="Guid id", new_args="id", ctor="Guid Id",
              nf_key="LocalizationKeys.Messages.UserNotFound", nf_arg="request.Id"),
            E("User", "CreateUser", "command", "Create", "[HttpPost]",
              "UserDetailResponse", "wrap", [US],
              "_users.CreateAsync(request.Request, ct)",
              sig="CreateUserRequest request", new_args="request", ctor="CreateUserRequest Request"),
            E("User", "UpdateUser", "command", "Update", '[HttpPut("{id:guid}")]',
              "UserDetailResponse", "wrap", [US],
              "_users.UpdateAsync(request.Id, request.Request, ct)",
              sig="Guid id, UpdateUserRequest request", new_args="id, request", ctor="Guid Id, UpdateUserRequest Request"),
            E("User", "DeleteUser", "command", "Delete", '[HttpDelete("{id:guid}")]',
              "bool", "wrap", [US], "_users.DeleteAsync(request.Id, ct)",
              sig="Guid id", new_args="id", ctor="Guid Id"),
            E("User", "ChangeUserPassword", "command", "ChangePassword", '[HttpPut("{id:guid}/password")]',
              "bool", "wrap", [US], "_users.ChangePasswordAsync(request.Id, request.Request, ct)",
              sig="Guid id, ChangePasswordRequest request", new_args="id, request",
              ctor="Guid Id, ChangePasswordRequest Request"),
            E("User", "GetUserAccess", "query", "GetAccess", '[HttpGet("{id:guid}/access")]',
              "UserAccessResponse", "notfound_throw", [US], "_users.GetAccessAsync(request.Id, ct)",
              sig="Guid id", new_args="id", ctor="Guid Id",
              nf_key="LocalizationKeys.Messages.UserNotFound", nf_arg="request.Id"),
            E("User", "SetUserAccess", "command", "SetAccess", '[HttpPut("{id:guid}/access")]',
              "UserAccessResponse", "wrap", [US], "_users.SetAccessAsync(request.Id, request.Request, ct)",
              sig="Guid id, SetUserAccessRequest request", new_args="id, request",
              ctor="Guid Id, SetUserAccessRequest Request"),
            E("User", "GetUserProfileImage", "query", "GetProfileImage", '[HttpGet("{id:guid}/profile-image")]',
              "ProfileImageResponse", "file", [US], "_users.GetProfileImageAsync(request.Id, ct)",
              sig="Guid id", new_args="id", ctor="Guid Id", file_args="result.Content, result.ContentType"),
            E("User", "SetUserProfileImage", "command", "SetProfileImage", '[HttpPut("{id:guid}/profile-image")]',
              "bool", "bool_notfound", [US],
              "_users.SetProfileImageAsync(request.Id, Convert.FromBase64String(request.Request.ContentBase64), request.Request.ContentType, ct)",
              sig="Guid id, SetProfileImageRequest request", new_args="id, request",
              ctor="Guid Id, SetProfileImageRequest Request",
              nf_key="LocalizationKeys.Messages.UserNotFound", nf_arg="request.Id"),
            E("User", "RemoveUserProfileImage", "command", "RemoveProfileImage", '[HttpDelete("{id:guid}/profile-image")]',
              "bool", "bool_notfound", [US], "_users.RemoveProfileImageAsync(request.Id, ct)",
              sig="Guid id", new_args="id", ctor="Guid Id",
              nf_key="LocalizationKeys.Messages.UserNotFound", nf_arg="request.Id"),
        ],
    },

    # ===================== IAM: Roles =====================
    {
        "controller": "RolesController", "module": "IAM", "feature_module": "IAM",
        "route": "api/v{version:apiVersion}/roles",
        "summary": "Rol yönetimi uç noktaları (IAM).",
        "model_usings": [CR, IDR, IDP], "ctrl_usings": [CR, IDR, IDP],
        "endpoints": [
            E("Role", "GetRoleList", "query", "GetAll", "[HttpGet]",
              "PaginatedResponse<RoleSummaryResponse>", "wrap", [RS],
              "_roles.GetAllAsync(request.Request, ct)",
              sig="[FromQuery] PaginatedRequest request", new_args="request", ctor="PaginatedRequest Request"),
            E("Role", "GetRoleById", "query", "GetById", '[HttpGet("{id:guid}")]',
              "RoleDetailResponse", "notfound_fail", [RS], "_roles.GetByIdAsync(request.Id, ct)",
              sig="Guid id", new_args="id", ctor="Guid Id", fail_msg='"Role not found."'),
            E("Role", "CreateRole", "command", "Create", "[HttpPost]",
              "RoleDetailResponse", "wrap", [RS], "_roles.CreateAsync(request.Request, ct)",
              sig="CreateRoleRequest request", new_args="request", ctor="CreateRoleRequest Request"),
            E("Role", "UpdateRole", "command", "Update", '[HttpPut("{id:guid}")]',
              "RoleDetailResponse", "wrap", [RS], "_roles.UpdateAsync(request.Id, request.Request, ct)",
              sig="Guid id, UpdateRoleRequest request", new_args="id, request", ctor="Guid Id, UpdateRoleRequest Request"),
            E("Role", "DeleteRole", "command", "Delete", '[HttpDelete("{id:guid}")]',
              "bool", "wrap", [RS], "_roles.DeleteAsync(request.Id, ct)",
              sig="Guid id", new_args="id", ctor="Guid Id"),
            E("Role", "SetRolePermissions", "command", "SetPermissions", '[HttpPut("{id:guid}/permissions")]',
              "RoleDetailResponse", "wrap", [RS], "_roles.SetPermissionsAsync(request.Id, request.Request, ct)",
              sig="Guid id, SetRolePermissionsRequest request", new_args="id, request",
              ctor="Guid Id, SetRolePermissionsRequest Request"),
        ],
    },

    # ===================== IAM: Permissions =====================
    {
        "controller": "PermissionsController", "module": "IAM", "feature_module": "IAM",
        "route": "api/v{version:apiVersion}/permissions",
        "summary": "Permission kataloğu uç noktaları (IAM).",
        "model_usings": [IDP], "ctrl_usings": [IDP],
        "endpoints": [
            E("Permission", "GetPermissionList", "query", "GetAll", "[HttpGet]",
              "IReadOnlyList<PermissionResponse>", "wrap", [PS], "_permissions.GetAllAsync(ct)"),
            E("Permission", "GetPermissionByCode", "query", "GetByCode", '[HttpGet("{code}")]',
              "PermissionResponse", "notfound_fail", [PS], "_permissions.GetByCodeAsync(request.Code, ct)",
              sig="string code", new_args="code", ctor="string Code", fail_msg='"Permission not found."'),
        ],
    },

    # ===================== IAM: Menus =====================
    {
        "controller": "MenusController", "module": "IAM", "feature_module": "IAM",
        "route": "api/v{version:apiVersion}/menus",
        "summary": "Menü yönetimi uç noktaları (IAM).",
        "model_usings": [CR, SYR, SYP], "ctrl_usings": [CR, SYR, SYP],
        "endpoints": [
            E("Menu", "GetMenuList", "query", "GetAll", "[HttpGet]",
              "PaginatedResponse<MenuResponse>", "wrap", [MS], "_menus.GetAllAsync(request.Request, ct)",
              sig="[FromQuery] PaginatedRequest request", new_args="request", ctor="PaginatedRequest Request"),
            E("Menu", "GetMenuById", "query", "GetById", '[HttpGet("{id:guid}")]',
              "MenuResponse", "notfound_throw", [MS], "_menus.GetByIdAsync(request.Id, ct)",
              sig="Guid id", new_args="id", ctor="Guid Id",
              nf_key="LocalizationKeys.Messages.MenuNotFound", nf_arg="request.Id"),
            E("Menu", "GetMyMenu", "query", "GetMyMenu", '[HttpGet("me")]',
              "IReadOnlyList<MenuTreeNodeResponse>", "wrap", [MS],
              "_menus.GetTreeForUserAsync(_currentUser.UserId, ct)",
              current_user=True, no_user_var=True),
            E("Menu", "CreateMenu", "command", "Create", "[HttpPost]",
              "MenuResponse", "wrap", [MS], "_menus.CreateAsync(request.Request, ct)",
              sig="CreateMenuRequest request", new_args="request", ctor="CreateMenuRequest Request"),
            E("Menu", "UpdateMenu", "command", "Update", '[HttpPut("{id:guid}")]',
              "MenuResponse", "wrap", [MS], "_menus.UpdateAsync(request.Id, request.Request, ct)",
              sig="Guid id, UpdateMenuRequest request", new_args="id, request", ctor="Guid Id, UpdateMenuRequest Request"),
            E("Menu", "DeleteMenu", "command", "Delete", '[HttpDelete("{id:guid}")]',
              "bool", "wrap", [MS], "_menus.DeleteAsync(request.Id, ct)",
              sig="Guid id", new_args="id", ctor="Guid Id"),
        ],
    },

    # ===================== IAM: ApiEndpoints =====================
    {
        "controller": "ApiEndpointsController", "module": "IAM", "feature_module": "IAM",
        "route": "api/v{version:apiVersion}/api-endpoints",
        "summary": "API endpoint kataloğu uç noktaları (IAM).",
        "model_usings": [CR, SYR, SYP], "ctrl_usings": [CR, SYR, SYP],
        "endpoints": [
            E("ApiEndpoint", "GetApiEndpointList", "query", "GetAll", "[HttpGet]",
              "PaginatedResponse<ApiEndpointResponse>", "wrap", [AES], "_endpoints.GetAllAsync(request.Request, ct)",
              sig="[FromQuery] PaginatedRequest request", new_args="request", ctor="PaginatedRequest Request"),
            E("ApiEndpoint", "GetApiEndpointById", "query", "GetById", '[HttpGet("{id:guid}")]',
              "ApiEndpointResponse", "notfound_fail", [AES], "_endpoints.GetByIdAsync(request.Id, ct)",
              sig="Guid id", new_args="id", ctor="Guid Id", fail_msg='"Endpoint not found."'),
            E("ApiEndpoint", "CreateApiEndpoint", "command", "Create", "[HttpPost]",
              "ApiEndpointResponse", "wrap", [AES], "_endpoints.CreateAsync(request.Request, ct)",
              sig="CreateApiEndpointRequest request", new_args="request", ctor="CreateApiEndpointRequest Request"),
            E("ApiEndpoint", "UpdateApiEndpoint", "command", "Update", '[HttpPut("{id:guid}")]',
              "ApiEndpointResponse", "wrap", [AES], "_endpoints.UpdateAsync(request.Id, request.Request, ct)",
              sig="Guid id, UpdateApiEndpointRequest request", new_args="id, request",
              ctor="Guid Id, UpdateApiEndpointRequest Request"),
            E("ApiEndpoint", "DeleteApiEndpoint", "command", "Delete", '[HttpDelete("{id:guid}")]',
              "bool", "wrap", [AES], "_endpoints.DeleteAsync(request.Id, ct)",
              sig="Guid id", new_args="id", ctor="Guid Id"),
        ],
    },

    # ===================== Core: AuditLogs =====================
    {
        "controller": "AuditLogsController", "module": "Core", "feature_module": "Core",
        "route": "api/v{version:apiVersion}/audit-logs",
        "summary": "Denetim kaydı (audit log) sorgu/ingest uç noktaları (Core).",
        "model_usings": [CR, LGR, LGP, "Energy.Shared.Identity"],
        "ctrl_usings": [CR, LGR, LGP],
        "endpoints": [
            E("Auditing", "QueryAuditLogs", "query", "Query", "[HttpGet]",
              "PaginatedResponse<AuditLogResponse>", "wrap", [ALS],
              "_logs.QueryAsync(request.Query, request.Paging, ct)",
              sig="[FromQuery] AuditLogQueryRequest query, [FromQuery] PaginatedRequest paging",
              new_args="query, paging", ctor="AuditLogQueryRequest Query, PaginatedRequest Paging"),
            E("Auditing", "GetAuditLogById", "query", "GetById", '[HttpGet("{id:long}")]',
              "AuditLogResponse", "notfound_throw", [ALS], "_logs.GetByIdAsync(request.Id, ct)",
              sig="long id", new_args="id", ctor="long Id",
              nf_key="LocalizationKeys.Messages.LogEntryNotFound", nf_arg="request.Id"),
            E("Auditing", "IngestAuditLog", "command", "Ingest", "[HttpPost]",
              "bool", "ingest", [ALS], "",
              sig="CreateAuditLogRequest request",
              new_args="request, HttpContext.Connection.RemoteIpAddress?.ToString()",
              ctor="CreateAuditLogRequest Request, string? IpAddress", current_user=True),
        ],
    },

    # ===================== Core: Localization =====================
    {
        "controller": "LocalizationController", "module": "Core", "feature_module": "Core",
        "route": "api/v{version:apiVersion}/localization",
        "summary": "Çok dilli metin kaynakları uç noktaları (Core).",
        "model_usings": [LCR, LCP], "ctrl_usings": [LCR, LCP],
        "endpoints": [
            E("Localization", "GetLocalizationEntries", "query", "GetAll", "[HttpGet]",
              "IReadOnlyList<LocalizationEntryResponse>", "wrap", [LS], "_localization.GetAllAsync(ct)"),
            E("Localization", "GetLocalizationByKey", "query", "GetByKey", '[HttpGet("{key}")]',
              "LocalizationEntryResponse", "notfound_fail", [LS], "_localization.GetByKeyAsync(request.Key, ct)",
              sig="string key", new_args="key", ctor="string Key", fail_msg='"Key not found."'),
            E("Localization", "UpsertLocalizationEntry", "command", "Upsert", "[HttpPost]",
              "LocalizationEntryResponse", "wrap", [LS], "_localization.UpsertAsync(request.Request, ct)",
              sig="UpsertLocalizationEntryRequest request", new_args="request",
              ctor="UpsertLocalizationEntryRequest Request"),
            E("Localization", "DeleteLocalizationEntry", "command", "Delete", '[HttpDelete("{key}")]',
              "bool", "wrap", [LS], "_localization.DeleteAsync(request.Key, ct)",
              sig="string key", new_args="key", ctor="string Key"),
        ],
    },

    # ===================== Core: Settings =====================
    {
        "controller": "SettingsController", "module": "Core", "feature_module": "Core",
        "route": "api/v{version:apiVersion}/settings",
        "summary": "Self-servis kullanıcı ayarları uç noktaları (Core).",
        "model_usings": [STR, STP], "ctrl_usings": [STR, STP],
        "endpoints": [
            E("UserSettings", "GetMySettings", "query", "GetMine", '[HttpGet("me")]',
              "UserSettingsResponse", "wrap", [STS], "_settings.GetAsync(currentUserId, ct)",
              current_user=True),
            E("UserSettings", "UpdateMySettings", "command", "UpdateMine", '[HttpPut("me")]',
              "UserSettingsResponse", "wrap", [STS], "_settings.UpdateAsync(currentUserId, request.Request, ct)",
              sig="UpdateUserSettingsRequest request", new_args="request",
              ctor="UpdateUserSettingsRequest Request", current_user=True),
        ],
    },

    # ===================== Core: Seeding =====================
    {
        "controller": "SeedController", "module": "Core", "feature_module": "Core",
        "route": "api/v{version:apiVersion}/seed",
        "summary": "İdempotent veri tohumlama uç noktaları (Core).",
        "model_usings": [], "ctrl_usings": [],
        "endpoints": [
            E("Seeding", "SeedAll", "command", "SeedAll", "[HttpPost]",
              "bool", "void_true", [SEED], "_seeder.SeedAllAsync(ct)"),
            E("Seeding", "SeedLocalization", "command", "SeedLocalization", '[HttpPost("localization")]',
              "SeedResultResponse", "wrap", [LS], "_localization.SeedFromResourcesAsync(ct)"),
            E("Seeding", "SeedLocalizationFromResx", "command", "SeedLocalizationFromResx", '[HttpPost("localization/resx")]',
              "SeedResultResponse", "wrap", [LS], "_localization.ImportFromResxAsync(ct)"),
        ],
    },

    # ===================== Chat =====================
    {
        "controller": "ChatController", "module": "Chat", "feature_module": "Chat",
        "route": "api/v{version:apiVersion}/chat",
        "summary": "Sohbet uç noktaları (kişisel + grup, mesaj, reaksiyon, ek).",
        "model_usings": [CHR, CHP], "ctrl_usings": [CHR, CHP],
        "endpoints": [
            E("Messaging", "GetChatContacts", "query", "GetContacts", '[HttpGet("contacts")]',
              "IReadOnlyList<ChatContactResponse>", "wrap", [CS], "_chat.GetContactsAsync(currentUserId, ct)",
              current_user=True),
            E("Messaging", "GetChatConversation", "query", "GetConversation", '[HttpGet("conversation/{peerId:guid}")]',
              "IReadOnlyList<ChatMessageResponse>", "wrap", [CS], "_chat.GetConversationAsync(currentUserId, request.PeerId, ct)",
              sig="Guid peerId", new_args="peerId", ctor="Guid PeerId", current_user=True),
            E("Messaging", "SendChatMessage", "command", "Send", '[HttpPost("messages")]',
              "ChatMessageResponse", "wrap", [CS], "_chat.SendAsync(currentUserId, request.Request, ct)",
              sig="SendChatMessageRequest request", new_args="request", ctor="SendChatMessageRequest Request",
              current_user=True),
            E("Messaging", "DeleteChatMessage", "command", "DeleteMessage", '[HttpDelete("messages/{messageId:guid}")]',
              "ChatMessageResponse", "notfound_fail", [CS], "_chat.DeleteMessageAsync(currentUserId, request.MessageId, ct)",
              sig="Guid messageId", new_args="messageId", ctor="Guid MessageId", current_user=True,
              fail_msg='"Message not found."'),
            E("Messaging", "ForwardChatMessage", "command", "Forward", '[HttpPost("messages/{messageId:guid}/forward")]',
              "ChatMessageResponse", "notfound_fail", [CS], "_chat.ForwardAsync(currentUserId, request.Request, ct)",
              sig="Guid messageId, ForwardChatMessageRequest request", new_args="messageId, request",
              ctor="Guid MessageId, ForwardChatMessageRequest Request", current_user=True,
              pre_call="request.Request.MessageId = request.MessageId;", fail_msg='"Message not found."'),
            E("Messaging", "ReactChatMessage", "command", "React", '[HttpPost("messages/{messageId:guid}/react")]',
              "ChatMessageResponse", "notfound_fail", [CS],
              "_chat.ToggleReactionAsync(currentUserId, request.MessageId, request.Request.Emoji, ct)",
              sig="Guid messageId, ReactChatMessageRequest request", new_args="messageId, request",
              ctor="Guid MessageId, ReactChatMessageRequest Request", current_user=True,
              fail_msg='"Message not found."'),
            E("Messaging", "GetChatAttachment", "query", "GetAttachment", '[HttpGet("messages/{messageId:guid}/attachment")]',
              "ChatAttachmentResponse", "file", [CS], "_chat.GetAttachmentAsync(currentUserId, request.MessageId, ct)",
              sig="Guid messageId", new_args="messageId", ctor="Guid MessageId", current_user=True,
              file_args="result.Content, result.ContentType, result.FileName"),
            E("Messaging", "GetChatUserAvatar", "query", "GetUserAvatar", '[HttpGet("users/{userId:guid}/avatar")]',
              "ChatAttachmentResponse", "file", [CS], "_chat.GetUserAvatarAsync(request.UserId, ct)",
              sig="Guid userId", new_args="userId", ctor="Guid UserId",
              file_args="result.Content, result.ContentType"),
            E("Messaging", "MarkChatRead", "command", "MarkRead", '[HttpPost("conversation/{peerId:guid}/read")]',
              "int", "wrap", [CS], "_chat.MarkReadAsync(currentUserId, request.PeerId, ct)",
              sig="Guid peerId", new_args="peerId", ctor="Guid PeerId", current_user=True),
            E("Messaging", "GetChatUnreadCount", "query", "UnreadCount", '[HttpGet("unread-count")]',
              "int", "wrap", [CS], "_chat.GetUnreadCountAsync(currentUserId, ct)", current_user=True),
            E("Messaging", "GetChatGroups", "query", "GetGroups", '[HttpGet("groups")]',
              "IReadOnlyList<ChatGroupResponse>", "wrap", [CS], "_chat.GetGroupsAsync(currentUserId, ct)",
              current_user=True),
            E("Messaging", "GetChatGroupInvites", "query", "GetGroupInvites", '[HttpGet("groups/invites")]',
              "IReadOnlyList<ChatGroupInviteResponse>", "wrap", [CS], "_chat.GetGroupInvitesAsync(currentUserId, ct)",
              current_user=True),
            E("Messaging", "CreateChatGroup", "command", "CreateGroup", '[HttpPost("groups")]',
              "ChatGroupResponse", "wrap", [CS], "_chat.CreateGroupAsync(currentUserId, request.Request, ct)",
              sig="CreateChatGroupRequest request", new_args="request", ctor="CreateChatGroupRequest Request",
              current_user=True),
            E("Messaging", "InviteToChatGroup", "command", "InviteToGroup", '[HttpPost("groups/{groupId:guid}/invite")]',
              "IReadOnlyList<Guid>", "wrap", [CS], "_chat.InviteToGroupAsync(currentUserId, request.GroupId, request.Request, ct)",
              sig="Guid groupId, InviteToGroupRequest request", new_args="groupId, request",
              ctor="Guid GroupId, InviteToGroupRequest Request", current_user=True),
            E("Messaging", "RespondChatGroupInvite", "command", "RespondInvite", '[HttpPost("groups/{groupId:guid}/respond")]',
              "bool", "wrap", [CS], "_chat.RespondInviteAsync(currentUserId, request.GroupId, request.Request.Accept, ct)",
              sig="Guid groupId, RespondGroupInviteRequest request", new_args="groupId, request",
              ctor="Guid GroupId, RespondGroupInviteRequest Request", current_user=True),
            E("Messaging", "GetChatGroupMembers", "query", "GetGroupMembers", '[HttpGet("groups/{groupId:guid}/members")]',
              "IReadOnlyList<ChatGroupMemberResponse>", "wrap", [CS], "_chat.GetGroupMembersAsync(currentUserId, request.GroupId, ct)",
              sig="Guid groupId", new_args="groupId", ctor="Guid GroupId", current_user=True),
            E("Messaging", "GetChatGroupMemberIds", "query", "GetGroupMemberIds", '[HttpGet("groups/{groupId:guid}/member-ids")]',
              "IReadOnlyList<Guid>", "wrap", [CS], "_chat.GetGroupMemberIdsAsync(request.GroupId, ct)",
              sig="Guid groupId", new_args="groupId", ctor="Guid GroupId"),
            E("Messaging", "GetChatGroupConversation", "query", "GetGroupConversation", '[HttpGet("groups/{groupId:guid}/conversation")]',
              "IReadOnlyList<ChatMessageResponse>", "wrap", [CS], "_chat.GetGroupConversationAsync(currentUserId, request.GroupId, ct)",
              sig="Guid groupId", new_args="groupId", ctor="Guid GroupId", current_user=True),
            E("Messaging", "DeleteChatGroup", "command", "DeleteGroup", '[HttpDelete("groups/{groupId:guid}")]',
              "bool", "bool_fail", [CS], "_chat.DeleteGroupAsync(currentUserId, request.GroupId, ct)",
              sig="Guid groupId", new_args="groupId", ctor="Guid GroupId", current_user=True,
              fail_msg='"Group not found or not permitted."'),
            E("Messaging", "RemoveChatGroupMember", "command", "RemoveMember", '[HttpDelete("groups/{groupId:guid}/members/{userId:guid}")]',
              "bool", "bool_fail", [CS], "_chat.RemoveMemberAsync(currentUserId, request.GroupId, request.UserId, ct)",
              sig="Guid groupId, Guid userId", new_args="groupId, userId", ctor="Guid GroupId, Guid UserId",
              current_user=True, fail_msg='"Member not found or not permitted."'),
            E("Messaging", "SetChatGroupAdmin", "command", "SetGroupAdmin", '[HttpPost("groups/{groupId:guid}/members/{userId:guid}/admin")]',
              "bool", "bool_fail", [CS], "_chat.SetMemberAdminAsync(currentUserId, request.GroupId, request.UserId, request.Request.IsAdmin, ct)",
              sig="Guid groupId, Guid userId, SetGroupAdminRequest request", new_args="groupId, userId, request",
              ctor="Guid GroupId, Guid UserId, SetGroupAdminRequest Request", current_user=True,
              fail_msg='"Member not found or not permitted."'),
        ],
    },

    # ===================== Processes =====================
    {
        "controller": "GoodsReceiptProcessController", "module": "Procurement.Processes",
        "feature_module": "Procurement", "route": "api/v{version:apiVersion}/procurement/processes/goods-receipt",
        "summary": "Mal kabul süreci (irsaliyeyi stok girişine dönüştürür).",
        "model_usings": ["Energy.Shared.Models.V1.Procurement.Processes.GoodsReceipt.Requests"],
        "ctrl_usings": ["Energy.Shared.Models.V1.Procurement.Processes.GoodsReceipt.Requests"],
        "endpoints": [
            E("Processes/GoodsReceipt", "ExecuteGoodsReceipt", "command", "Execute", "[HttpPost]",
              "bool", "process", [GR], "",
              sig="[FromBody] GoodsReceiptProcessRequest request", new_args="request",
              ctor="GoodsReceiptProcessRequest Request",
              proc_body="            await _goodsReceipt.ReceiveAsync(request.Request.PurchaseReceiptId, ct);\n"
                        "            return BaseResponse<bool>.Success(true, \"Completed\");"),
        ],
    },
    {
        "controller": "StockIssueProcessController", "module": "Inventory.Processes",
        "feature_module": "Inventory", "route": "api/v{version:apiVersion}/inventory/processes/stock-issue",
        "summary": "Stok çıkış süreci (FIFO maliyetlendirme + stok hareketi).",
        "model_usings": ["Energy.Shared.Models.V1.Inventory.Processes.StockIssue.Requests",
                         "Energy.Shared.Models.V1.Inventory.Processes.StockIssue.Responses"],
        "ctrl_usings": ["Energy.Shared.Models.V1.Inventory.Processes.StockIssue.Requests",
                        "Energy.Shared.Models.V1.Inventory.Processes.StockIssue.Responses"],
        "endpoints": [
            E("Processes/StockIssue", "ExecuteStockIssue", "command", "Execute", "[HttpPost]",
              "StockIssueProcessResponse", "process", [INV], "",
              sig="[FromBody] StockIssueProcessRequest request", new_args="request",
              ctor="StockIssueProcessRequest Request",
              proc_body="            var result = await _inventory.PostStockOutAsync(\n"
                        "                new StockOutRequest(request.Request.WarehouseId, request.Request.MaterialId, request.Request.UnitOfMeasureId,\n"
                        "                    request.Request.Quantity, request.Request.ProjectId, request.Request.Note), ct);\n"
                        "            return BaseResponse<StockIssueProcessResponse>.Success(\n"
                        "                new StockIssueProcessResponse { TotalCost = result.TotalCost, AllocationCount = result.Allocations.Count },\n"
                        "                \"Completed\");"),
        ],
    },
    {
        "controller": "StockTransferProcessController", "module": "Inventory.Processes",
        "feature_module": "Inventory", "route": "api/v{version:apiVersion}/inventory/processes/stock-transfer",
        "summary": "Depolar arası stok transfer süreci (FIFO çıkış + giriş, tek işlem).",
        "model_usings": ["Energy.Shared.Models.V1.Inventory.Processes.StockTransfer.Requests",
                         "Energy.Shared.Models.V1.Inventory.Processes.StockTransfer.Responses"],
        "ctrl_usings": ["Energy.Shared.Models.V1.Inventory.Processes.StockTransfer.Requests",
                        "Energy.Shared.Models.V1.Inventory.Processes.StockTransfer.Responses"],
        "endpoints": [
            E("Processes/StockTransfer", "ExecuteStockTransfer", "command", "Execute", "[HttpPost]",
              "StockTransferProcessResponse", "process", [INV], "",
              sig="[FromBody] StockTransferProcessRequest request", new_args="request",
              ctor="StockTransferProcessRequest Request",
              proc_body="            var result = await _inventory.TransferAsync(\n"
                        "                new StockTransferRequest(request.Request.SourceWarehouseId, request.Request.TargetWarehouseId, request.Request.MaterialId,\n"
                        "                    request.Request.UnitOfMeasureId, request.Request.Quantity, request.Request.Note), ct);\n"
                        "            return BaseResponse<StockTransferProcessResponse>.Success(\n"
                        "                new StockTransferProcessResponse { TotalCost = result.TotalCost, AllocationCount = result.Allocations.Count },\n"
                        "                \"Completed\");"),
        ],
    },
    {
        "controller": "PaymentAllocationProcessController", "module": "Finance.Processes",
        "feature_module": "Finance", "route": "api/v{version:apiVersion}/finance/processes/payment-allocation",
        "summary": "Ödeme tahsis süreci (bir ödemeyi birden çok borca kapatır).",
        "model_usings": ["Energy.Shared.Models.V1.Finance.Processes.PaymentAllocation.Requests",
                         "Energy.Shared.Models.V1.Finance.Processes.PaymentAllocation.Responses"],
        "ctrl_usings": ["Energy.Shared.Models.V1.Finance.Processes.PaymentAllocation.Requests",
                        "Energy.Shared.Models.V1.Finance.Processes.PaymentAllocation.Responses"],
        "endpoints": [
            E("Processes/PaymentAllocation", "ExecutePaymentAllocation", "command", "Execute", "[HttpPost]",
              "PaymentAllocationProcessResponse", "process", [FIN], "",
              sig="[FromBody] PaymentAllocationProcessRequest request", new_args="request",
              ctor="PaymentAllocationProcessRequest Request",
              proc_body="            if (request.Request.Lines is null || request.Request.Lines.Count == 0)\n"
                        "            {\n"
                        "                return BaseResponse<PaymentAllocationProcessResponse>.Failure(\"At least one allocation line is required.\");\n"
                        "            }\n\n"
                        "            var lines = request.Request.Lines\n"
                        "                .Select(l => new FinanceAllocationLine(l.TargetId, l.Amount))\n"
                        "                .ToList();\n"
                        "            await _finance.AllocatePaymentAsync(request.Request.PaymentId, lines, ct);\n"
                        "            return BaseResponse<PaymentAllocationProcessResponse>.Success(\n"
                        "                new PaymentAllocationProcessResponse\n"
                        "                {\n"
                        "                    AllocatedLineCount = lines.Count,\n"
                        "                    TotalAllocated = lines.Sum(l => l.Amount),\n"
                        "                }, \"Completed\");"),
        ],
    },
    {
        "controller": "ProgressPaymentPostingProcessController", "module": "Finance.Processes",
        "feature_module": "Finance", "route": "api/v{version:apiVersion}/finance/processes/progress-payment-posting",
        "summary": "Hakediş muhasebeleştirme süreci (alacak/borç finansal hareketi).",
        "model_usings": ["Energy.Shared.Models.V1.Finance.Processes.ProgressPaymentPosting.Requests",
                         "Energy.Shared.Models.V1.Finance.Processes.ProgressPaymentPosting.Responses"],
        "ctrl_usings": ["Energy.Shared.Models.V1.Finance.Processes.ProgressPaymentPosting.Requests",
                        "Energy.Shared.Models.V1.Finance.Processes.ProgressPaymentPosting.Responses"],
        "endpoints": [
            E("Processes/ProgressPaymentPosting", "ExecuteProgressPaymentPosting", "command", "Execute", "[HttpPost]",
              "ProgressPaymentPostingProcessResponse", "process", [FIN], "",
              sig="[FromBody] ProgressPaymentPostingProcessRequest request", new_args="request",
              ctor="ProgressPaymentPostingProcessRequest Request",
              proc_body="            var id = await _finance.PostProgressPaymentAsync(request.Request.ProgressPaymentId, ct);\n"
                        "            return BaseResponse<ProgressPaymentPostingProcessResponse>.Success(\n"
                        "                new ProgressPaymentPostingProcessResponse { FinancialTransactionId = id }, \"Completed\");"),
        ],
    },
    {
        "controller": "TimesheetCostProcessController", "module": "Finance.Processes",
        "feature_module": "Finance", "route": "api/v{version:apiVersion}/finance/processes/timesheet-cost",
        "summary": "Puantaj maliyet süreci (HR maliyet finansal hareketi).",
        "model_usings": ["Energy.Shared.Models.V1.Finance.Processes.TimesheetCost.Requests",
                         "Energy.Shared.Models.V1.Finance.Processes.TimesheetCost.Responses"],
        "ctrl_usings": ["Energy.Shared.Models.V1.Finance.Processes.TimesheetCost.Requests",
                        "Energy.Shared.Models.V1.Finance.Processes.TimesheetCost.Responses"],
        "endpoints": [
            E("Processes/TimesheetCost", "ExecuteTimesheetCost", "command", "Execute", "[HttpPost]",
              "TimesheetCostProcessResponse", "process", [FIN], "",
              sig="[FromBody] TimesheetCostProcessRequest request", new_args="request",
              ctor="TimesheetCostProcessRequest Request",
              proc_body="            var id = await _finance.PostTimesheetCostAsync(request.Request.TimesheetId, request.Request.CurrencyId, ct);\n"
                        "            return BaseResponse<TimesheetCostProcessResponse>.Success(\n"
                        "                new TimesheetCostProcessResponse { FinancialTransactionId = id }, \"Completed\");"),
        ],
    },

    # ===================== Reports =====================
    report("Procurement.Reports", "PurchaseOrderSummaryController",
           "api/v{version:apiVersion}/procurement/reports/purchase-order-summary",
           "Reports/PurchaseOrderSummary", "IPurchaseOrderSummaryService",
           "Energy.Application.Modules.Procurement.Reports.PurchaseOrderSummary.Services",
           "PurchaseOrderSummaryRequest", "PurchaseOrderSummaryRowResponse", "purchase-order-summary.csv"),
    report("Inventory.Reports", "StockBalanceReportController",
           "api/v{version:apiVersion}/inventory/reports/stock-balance-report",
           "Reports/StockBalanceReport", "IStockBalanceReportService",
           "Energy.Application.Modules.Inventory.Reports.StockBalanceReport.Services",
           "StockBalanceReportRequest", "StockBalanceReportRowResponse", "stock-balance-report.csv"),
    report("HR.Reports", "TimesheetSummaryController",
           "api/v{version:apiVersion}/h-r/reports/timesheet-summary",
           "Reports/TimesheetSummary", "ITimesheetSummaryService",
           "Energy.Application.Modules.HR.Reports.TimesheetSummary.Services",
           "TimesheetSummaryRequest", "TimesheetSummaryRowResponse", "timesheet-summary.csv"),
    report("Finance.Reports", "PayableAgingController",
           "api/v{version:apiVersion}/finance/reports/payable-aging",
           "Reports/PayableAging", "IPayableAgingService",
           "Energy.Application.Modules.Finance.Reports.PayableAging.Services",
           "PayableAgingRequest", "PayableAgingRowResponse", "payable-aging.csv"),
    report("Finance.Reports", "ReceivableAgingController",
           "api/v{version:apiVersion}/finance/reports/receivable-aging",
           "Reports/ReceivableAging", "IReceivableAgingService",
           "Energy.Application.Modules.Finance.Reports.ReceivableAging.Services",
           "ReceivableAgingRequest", "ReceivableAgingRowResponse", "receivable-aging.csv"),
    report("ProgressPayments.Reports", "ProgressPaymentSummaryController",
           "api/v{version:apiVersion}/progress-payments/reports/progress-payment-summary",
           "Reports/ProgressPaymentSummary", "IProgressPaymentSummaryService",
           "Energy.Application.Modules.ProgressPayments.Reports.ProgressPaymentSummary.Services",
           "ProgressPaymentSummaryRequest", "ProgressPaymentSummaryRowResponse", "progress-payment-summary.csv"),
    report("Projects.Reports", "ProjectStatusReportController",
           "api/v{version:apiVersion}/projects/reports/project-status-report",
           "Reports/ProjectStatusReport", "IProjectStatusReportService",
           "Energy.Application.Modules.Projects.Reports.ProjectStatusReport.Services",
           "ProjectStatusReportRequest", "ProjectStatusReportRowResponse", "project-status-report.csv"),
]

