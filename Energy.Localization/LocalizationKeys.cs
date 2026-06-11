namespace Energy.Localization;

public static class LocalizationKeys
{
    public static class Common
    {
        public const string AppName = "Common.AppName";
        public const string Home = "Common.Home";
        public const string Privacy = "Common.Privacy";
        public const string Welcome = "Common.Welcome";
        public const string LearnMore = "Common.LearnMore";
        public const string Login = "Common.Login";
        public const string Save = "Common.Save";
        public const string Cancel = "Common.Cancel";
        public const string Delete = "Common.Delete";
        public const string Edit = "Common.Edit";
        public const string Users = "Common.Users";
        public const string Roles = "Common.Roles";
        public const string Projects = "Common.Projects";
        public const string PrivacyPolicy = "Common.PrivacyPolicy";
        public const string SitePrivacyDescription = "Common.SitePrivacyDescription";
        public const string ToggleNavigation = "Common.ToggleNavigation";
        public const string Turkish = "Common.Turkish";
        public const string English = "Common.English";
        public const string Error = "Common.Error";
        public const string RequestId = "Common.RequestId";
        public const string ErrorOccurred = "Common.ErrorOccurred";
    }

    public static class Roles
    {
        public const string AdminDisplayName = "Roles.Admin.DisplayName";
        public const string AdminDescription = "Roles.Admin.Description";
        public const string OperationsManagerDisplayName = "Roles.OperationsManager.DisplayName";
        public const string OperationsManagerDescription = "Roles.OperationsManager.Description";
        public const string LocalizationEditorDisplayName = "Roles.LocalizationEditor.DisplayName";
        public const string LocalizationEditorDescription = "Roles.LocalizationEditor.Description";
        public const string ReadOnlyDisplayName = "Roles.ReadOnly.DisplayName";
        public const string ReadOnlyDescription = "Roles.ReadOnly.Description";
    }

    public static class Users
    {
        public const string AdminFirstName = "Users.Admin.FirstName";
        public const string AdminLastName = "Users.Admin.LastName";
        public const string OperationsManagerFirstName = "Users.OperationsManager.FirstName";
        public const string OperationsManagerLastName = "Users.OperationsManager.LastName";
        public const string LocalizationEditorFirstName = "Users.LocalizationEditor.FirstName";
        public const string LocalizationEditorLastName = "Users.LocalizationEditor.LastName";
        public const string ReadOnlyFirstName = "Users.ReadOnly.FirstName";
        public const string ReadOnlyLastName = "Users.ReadOnly.LastName";
    }

    public static class Permissions
    {
        public const string ReadName = "Permissions.Default.Read.Name";
        public const string ReadAllName = "Permissions.Default.ReadAll.Name";
        public const string CreateName = "Permissions.Default.Create.Name";
        public const string UpdateName = "Permissions.Default.Update.Name";
        public const string DeleteName = "Permissions.Default.Delete.Name";

        public static class AccessRules
        {
            public const string ReadName = "Permissions.AccessRules.Read.Name";
            public const string ReadAllName = "Permissions.AccessRules.ReadAll.Name";
            public const string CreateName = "Permissions.AccessRules.Create.Name";
            public const string UpdateName = "Permissions.AccessRules.Update.Name";
            public const string DeleteName = "Permissions.AccessRules.Delete.Name";
            public const string ManagePermissionsName = "Permissions.AccessRules.ManagePermissions.Name";
        }

        public static class Home
        {
            public const string GetDashboardName = "Permissions.Home.GetDashboard.Name";
        }

        public static class User
        {
            public const string GetUsersName = "Permissions.User.GetUsers.Name";
            public const string GetUserName = "Permissions.User.GetUser.Name";
            public const string CreateUserName = "Permissions.User.CreateUser.Name";
            public const string UpdateUserName = "Permissions.User.UpdateUser.Name";
            public const string SetRolesName = "Permissions.User.SetRoles.Name";
            public const string UpdatePasswordName = "Permissions.User.UpdatePassword.Name";
            public const string DeleteUserName = "Permissions.User.DeleteUser.Name";
            public const string GetAdminPermissionHealthName = "Permissions.User.GetAdminPermissionHealth.Name";
        }

        public static class Permission
        {
            public const string GetPermissionsName = "Permissions.Permission.GetPermissions.Name";
            public const string GetPermissionName = "Permissions.Permission.GetPermission.Name";
            public const string CreatePermissionName = "Permissions.Permission.CreatePermission.Name";
            public const string UpdatePermissionName = "Permissions.Permission.UpdatePermission.Name";
            public const string DeletePermissionName = "Permissions.Permission.DeletePermission.Name";
        }

        public static class Role
        {
            public const string GetRolesName = "Permissions.Role.GetRoles.Name";
            public const string GetRoleName = "Permissions.Role.GetRole.Name";
            public const string CreateRoleName = "Permissions.Role.CreateRole.Name";
            public const string UpdateRoleName = "Permissions.Role.UpdateRole.Name";
            public const string DeleteRoleName = "Permissions.Role.DeleteRole.Name";
            public const string GetRolePermissionsName = "Permissions.Role.GetRolePermissions.Name";
            public const string SetRolePermissionsName = "Permissions.Role.SetRolePermissions.Name";
            public const string GetRoleMenusName = "Permissions.Role.GetRoleMenus.Name";
            public const string SetRoleMenusName = "Permissions.Role.SetRoleMenus.Name";
        }

        public static class Menu
        {
            public const string GetMenusName = "Permissions.Menu.GetMenus.Name";
            public const string GetMenuTreeName = "Permissions.Menu.GetMenuTree.Name";
            public const string GetMenuName = "Permissions.Menu.GetMenu.Name";
            public const string CreateMenuName = "Permissions.Menu.CreateMenu.Name";
            public const string UpdateMenuName = "Permissions.Menu.UpdateMenu.Name";
            public const string DeleteMenuName = "Permissions.Menu.DeleteMenu.Name";
            public const string GetMenuPermissionsName = "Permissions.Menu.GetMenuPermissions.Name";
            public const string SetMenuPermissionsName = "Permissions.Menu.SetMenuPermissions.Name";
        }

        public static class Localization
        {
            public const string GetAllName = "Permissions.Localization.GetAll.Name";
            public const string GetByKeyName = "Permissions.Localization.GetByKey.Name";
            public const string UpsertName = "Permissions.Localization.Upsert.Name";
            public const string DeleteName = "Permissions.Localization.Delete.Name";
        }

        public static class AccessRule
        {
            public const string GetAccessRulesName = "Permissions.AccessRule.GetAccessRules.Name";
            public const string GetAccessRuleName = "Permissions.AccessRule.GetAccessRule.Name";
            public const string CreateAccessRuleName = "Permissions.AccessRule.CreateAccessRule.Name";
            public const string UpdateAccessRuleName = "Permissions.AccessRule.UpdateAccessRule.Name";
            public const string DeleteAccessRuleName = "Permissions.AccessRule.DeleteAccessRule.Name";
            public const string GetAccessRulePermissionsName = "Permissions.AccessRule.GetAccessRulePermissions.Name";
            public const string SetAccessRulePermissionsName = "Permissions.AccessRule.SetAccessRulePermissions.Name";
            public const string GetRequiredPermissionsName = "Permissions.AccessRule.GetRequiredPermissions.Name";
        }
    }

    public static class Menus
    {
        public const string Dashboard = "Menus.Dashboard";
        public const string System = "Menus.System";
        public const string SystemUsers = "Menus.System.Users";
        public const string SystemRoles = "Menus.System.Roles";
        public const string SystemPermissions = "Menus.System.Permissions";
        public const string SystemMenus = "Menus.System.Menus";
        public const string SystemLocalization = "Menus.System.Localization";
        public const string SystemAccessRules = "Menus.System.AccessRules";
        public const string Profile = "Menus.Profile";
    }

    public static class Messages
    {
        public const string AdminUserCreationFailed = "Messages.AdminUserCreationFailed";
        public const string RoleNotFound = "Messages.RoleNotFound";
        public const string RoleAlreadyExists = "Messages.RoleAlreadyExists";
        public const string MenusNotFound = "Messages.MenusNotFound";
        public const string PermissionsNotFound = "Messages.PermissionsNotFound";
        public const string AccessRuleNotFound = "Messages.AccessRuleNotFound";
        public const string AccessRuleAlreadyExists = "Messages.AccessRuleAlreadyExists";
        public const string ScopeRequired = "Messages.ScopeRequired";
        public const string PathRequired = "Messages.PathRequired";
        public const string MenuNotFound = "Messages.MenuNotFound";
        public const string MenuUrlAlreadyExists = "Messages.MenuUrlAlreadyExists";
        public const string ParentMenuNotFound = "Messages.ParentMenuNotFound";
        public const string MenuSelfParent = "Messages.MenuSelfParent";
        public const string UserRolesNotFound = "Messages.UserRolesNotFound";
        public const string UserNotFound = "Messages.UserNotFound";
        public const string UserNameAlreadyExists = "Messages.UserNameAlreadyExists";
        public const string AuthenticationRequired = "Messages.AuthenticationRequired";
        public const string BearerTokenInvalidOrMissing = "Messages.BearerTokenInvalidOrMissing";
        public const string PermissionDeniedAction = "Messages.PermissionDeniedAction";
        public const string JwtConfigMissing = "Messages.JwtConfigMissing";
        public const string DefaultConnectionNotConfigured = "Messages.DefaultConnectionNotConfigured";
        public const string ApiBaseUrlNotConfigured = "Messages.ApiBaseUrlNotConfigured";
        public const string AccessTokenRequired = "Messages.AccessTokenRequired";
        public const string JwtKeyConfigInvalid = "Messages.JwtKeyConfigInvalid";
        public const string PermissionNotFound = "Messages.PermissionNotFound";
        public const string PermissionAlreadyExists = "Messages.PermissionAlreadyExists";
        public const string LocalizationKeyNotFound = "Messages.LocalizationKeyNotFound";
        public const string ValidationFailed = "Messages.ValidationFailed";
        public const string UnexpectedError = "Messages.UnexpectedError";
        public const string AccessRuleCentralValidationFailed = "Messages.AccessRuleCentralValidationFailed";
        public const string PayloadLoggingSkipped = "Messages.PayloadLoggingSkipped";
        public const string ApiResponseDeserializationFailed = "Messages.ApiResponseDeserializationFailed";
        public const string ApiResponseBodyEmpty = "Messages.ApiResponseBodyEmpty";
        public const string SwaggerJwtDescription = "Messages.SwaggerJwtDescription";
        public const string LoginRequestFailed = "Messages.LoginRequestFailed";
        public const string LoginResponseInvalid = "Messages.LoginResponseInvalid";
        public const string ResxUpsertFailed = "Messages.ResxUpsertFailed";
        public const string ResxDeleteFailed = "Messages.ResxDeleteFailed";
        public const string ProfileImageEmpty = "Messages.ProfileImageEmpty";
        public const string ProfileImageInvalidType = "Messages.ProfileImageInvalidType";
        public const string ProfileImageTooLarge = "Messages.ProfileImageTooLarge";
    }

    public static class Dashboard
    {
        public const string Title = "Dashboard.Title";
        public const string Subtitle = "Dashboard.Subtitle";
        public const string ConfigurationStatus = "Dashboard.ConfigurationStatus";
        public const string Ready = "Dashboard.Ready";
        public const string NeedsAttention = "Dashboard.NeedsAttention";
        public const string Readiness = "Dashboard.Readiness";
        public const string ReadinessDescription = "Dashboard.ReadinessDescription";
        public const string ModuleSummary = "Dashboard.ModuleSummary";
        public const string SystemOverview = "Dashboard.SystemOverview";
        public const string SystemOverviewDescription = "Dashboard.SystemOverviewDescription";
        public const string QuickActions = "Dashboard.QuickActions";
        public const string QuickActionsDescription = "Dashboard.QuickActionsDescription";
        public const string EmptyState = "Dashboard.EmptyState";
        public const string ActiveUsers = "Dashboard.ActiveUsers";
        public const string ActiveUsersDescription = "Dashboard.ActiveUsersDescription";
        public const string TotalRoles = "Dashboard.TotalRoles";
        public const string TotalRolesDescription = "Dashboard.TotalRolesDescription";
        public const string TotalPermissions = "Dashboard.TotalPermissions";
        public const string TotalPermissionsDescription = "Dashboard.TotalPermissionsDescription";
        public const string TotalMenus = "Dashboard.TotalMenus";
        public const string TotalMenusDescription = "Dashboard.TotalMenusDescription";
    }

    public static class Auth
    {
        public const string SignInTitle = "Auth.SignInTitle";
        public const string SignInSubtitle = "Auth.SignInSubtitle";
        public const string UserNameOrEmail = "Auth.UserNameOrEmail";
        public const string Password = "Auth.Password";
        public const string ShowPassword = "Auth.ShowPassword";
        public const string HidePassword = "Auth.HidePassword";
        public const string RememberMe = "Auth.RememberMe";
        public const string SignIn = "Auth.SignIn";
        public const string SignOut = "Auth.SignOut";
        public const string InvalidCredentials = "Auth.InvalidCredentials";
        public const string SessionExpired = "Auth.SessionExpired";
        public const string AccessDeniedTitle = "Auth.AccessDeniedTitle";
        public const string AccessDeniedDescription = "Auth.AccessDeniedDescription";
        public const string AccessDeniedRequestTitle = "Auth.AccessDeniedRequestTitle";
        public const string AccessDeniedRequestTarget = "Auth.AccessDeniedRequestTarget";
        public const string AccessDeniedRequestHowTo = "Auth.AccessDeniedRequestHowTo";
        public const string AccessDeniedRequestedUrl = "Auth.AccessDeniedRequestedUrl";
        public const string AccessDeniedPermissionCode = "Auth.AccessDeniedPermissionCode";
        public const string BackToLogin = "Auth.BackToLogin";
        public const string BackToDashboard = "Auth.BackToDashboard";
        public const string SignedInAs = "Auth.SignedInAs";
        public const string FieldRequired = "Auth.FieldRequired";
        public const string SigningIn = "Auth.SigningIn";
        public const string AccountLockedOut = "Auth.AccountLockedOut";
        public const string InvalidCredentialsDetail = "Auth.InvalidCredentialsDetail";
        public const string LoginSuccessful = "Auth.LoginSuccessful";
    }

    public static class Layout
    {
        public const string ToggleMenu = "Layout.ToggleMenu";
        public const string Language = "Layout.Language";
        public const string UserMenu = "Layout.UserMenu";
        public const string FooterCopyright = "Layout.FooterCopyright";
        public const string Loading = "Layout.Loading";
        public const string Profile = "Layout.Profile";
        public const string MyProfile = "Layout.MyProfile";
    }

    public static class ProfileScreen
    {
        public const string Title = "ProfileScreen.Title";
        public const string Subtitle = "ProfileScreen.Subtitle";
        public const string PersonalInfo = "ProfileScreen.PersonalInfo";
        public const string AccountInfo = "ProfileScreen.AccountInfo";
        public const string Roles = "ProfileScreen.Roles";
        public const string Permissions = "ProfileScreen.Permissions";
        public const string ProfileImage = "ProfileScreen.ProfileImage";
        public const string UploadImage = "ProfileScreen.UploadImage";
        public const string RemoveImage = "ProfileScreen.RemoveImage";
        public const string ImageUploaded = "ProfileScreen.ImageUploaded";
        public const string ImageRemoved = "ProfileScreen.ImageRemoved";
        public const string ImageHint = "ProfileScreen.ImageHint";
        public const string NoRoles = "ProfileScreen.NoRoles";
        public const string NoPermissions = "ProfileScreen.NoPermissions";
        public const string AccountActive = "ProfileScreen.AccountActive";
        public const string AccountInactive = "ProfileScreen.AccountInactive";
    }

    public static class Grid
    {
        public const string Add = "Grid.Add";
        public const string Edit = "Grid.Edit";
        public const string Delete = "Grid.Delete";
        public const string Save = "Grid.Save";
        public const string Cancel = "Grid.Cancel";
        public const string Refresh = "Grid.Refresh";
        public const string Search = "Grid.Search";
        public const string NoData = "Grid.NoData";
        public const string ConfirmDelete = "Grid.ConfirmDelete";
        public const string Actions = "Grid.Actions";
        public const string Loading = "Grid.Loading";
        public const string Yes = "Grid.Yes";
        public const string No = "Grid.No";
    }

    public static class Notifications
    {
        public const string Saved = "Notifications.Saved";
        public const string Deleted = "Notifications.Deleted";
        public const string Failed = "Notifications.Failed";
        public const string Unauthorized = "Notifications.Unauthorized";
        public const string Forbidden = "Notifications.Forbidden";
        public const string NetworkError = "Notifications.NetworkError";
        public const string GenericError = "Notifications.GenericError";
    }

    public static class UsersScreen
    {
        public const string Title = "UsersScreen.Title";
        public const string Subtitle = "UsersScreen.Subtitle";
        public const string FirstName = "UsersScreen.FirstName";
        public const string LastName = "UsersScreen.LastName";
        public const string UserName = "UsersScreen.UserName";
        public const string Email = "UsersScreen.Email";
        public const string PhoneNumber = "UsersScreen.PhoneNumber";
        public const string IsActive = "UsersScreen.IsActive";
        public const string EmailConfirmed = "UsersScreen.EmailConfirmed";
        public const string PhoneNumberConfirmed = "UsersScreen.PhoneNumberConfirmed";
        public const string TwoFactorEnabled = "UsersScreen.TwoFactorEnabled";
        public const string LockoutEnabled = "UsersScreen.LockoutEnabled";
        public const string LockoutEnd = "UsersScreen.LockoutEnd";
        public const string Password = "UsersScreen.Password";
        public const string Roles = "UsersScreen.Roles";
        public const string ManageRoles = "UsersScreen.ManageRoles";
        public const string ChangePassword = "UsersScreen.ChangePassword";
        public const string CreateTitle = "UsersScreen.CreateTitle";
        public const string EditTitle = "UsersScreen.EditTitle";
        public const string PasswordTitle = "UsersScreen.PasswordTitle";
        public const string RolesTitle = "UsersScreen.RolesTitle";
    }

    public static class RolesScreen
    {
        public const string Title = "RolesScreen.Title";
        public const string Subtitle = "RolesScreen.Subtitle";
        public const string Name = "RolesScreen.Name";
        public const string Description = "RolesScreen.Description";
        public const string AssignedUserCount = "RolesScreen.AssignedUserCount";
        public const string CreateTitle = "RolesScreen.CreateTitle";
        public const string EditTitle = "RolesScreen.EditTitle";
        public const string ManagePermissions = "RolesScreen.ManagePermissions";
        public const string ManageMenus = "RolesScreen.ManageMenus";
        public const string PermissionsTitle = "RolesScreen.PermissionsTitle";
        public const string MenusTitle = "RolesScreen.MenusTitle";
    }

    public static class PermissionsScreen
    {
        public const string Title = "PermissionsScreen.Title";
        public const string Subtitle = "PermissionsScreen.Subtitle";
        public const string Code = "PermissionsScreen.Code";
        public const string Name = "PermissionsScreen.Name";
        public const string CreateTitle = "PermissionsScreen.CreateTitle";
        public const string EditTitle = "PermissionsScreen.EditTitle";
        public const string SeedDefaults = "PermissionsScreen.SeedDefaults";
    }

    public static class MenusScreen
    {
        public const string Title = "MenusScreen.Title";
        public const string Subtitle = "MenusScreen.Subtitle";
        public const string Name = "MenusScreen.Name";
        public const string NameKey = "MenusScreen.NameKey";
        public const string Url = "MenusScreen.Url";
        public const string Icon = "MenusScreen.Icon";
        public const string Order = "MenusScreen.Order";
        public const string Parent = "MenusScreen.Parent";
        public const string CreateTitle = "MenusScreen.CreateTitle";
        public const string EditTitle = "MenusScreen.EditTitle";
        public const string SeedDefaults = "MenusScreen.SeedDefaults";
        public const string NoParent = "MenusScreen.NoParent";
    }

    public static class LocalizationScreen
    {
        public const string Title = "LocalizationScreen.Title";
        public const string Subtitle = "LocalizationScreen.Subtitle";
        public const string Key = "LocalizationScreen.Key";
        public const string TurkishValue = "LocalizationScreen.TurkishValue";
        public const string EnglishValue = "LocalizationScreen.EnglishValue";
        public const string InvariantValue = "LocalizationScreen.InvariantValue";
        public const string CreateTitle = "LocalizationScreen.CreateTitle";
        public const string EditTitle = "LocalizationScreen.EditTitle";
        public const string ImportFromResx = "LocalizationScreen.ImportFromResx";
        public const string ImportConfirmTitle = "LocalizationScreen.ImportConfirmTitle";
        public const string ImportConfirmMessage = "LocalizationScreen.ImportConfirmMessage";
        public const string Imported = "LocalizationScreen.Imported";
        public const string EntrySaved = "LocalizationScreen.EntrySaved";
        public const string EntryDeleted = "LocalizationScreen.EntryDeleted";
        public const string KeyNotFound = "LocalizationScreen.KeyNotFound";
    }

    public static class AccessRulesScreen
    {
        public const string Title = "AccessRulesScreen.Title";
        public const string Subtitle = "AccessRulesScreen.Subtitle";
        public const string Name = "AccessRulesScreen.Name";
        public const string Scope = "AccessRulesScreen.Scope";
        public const string ScopeApi = "AccessRulesScreen.Scope.API";
        public const string ScopeWeb = "AccessRulesScreen.Scope.Web";
        public const string Path = "AccessRulesScreen.Path";
        public const string PathHint = "AccessRulesScreen.PathHint";
        public const string HttpMethod = "AccessRulesScreen.HttpMethod";
        public const string HttpMethodAll = "AccessRulesScreen.HttpMethod.All";
        public const string Description = "AccessRulesScreen.Description";
        public const string IsEnabled = "AccessRulesScreen.IsEnabled";
        public const string CreateTitle = "AccessRulesScreen.CreateTitle";
        public const string EditTitle = "AccessRulesScreen.EditTitle";
        public const string ManagePermissions = "AccessRulesScreen.ManagePermissions";
        public const string PermissionsTitle = "AccessRulesScreen.PermissionsTitle";
        public const string PatternHelp = "AccessRulesScreen.PatternHelp";
        public const string PatternExamples = "AccessRulesScreen.PatternExamples";
    }

    /// <summary>
    /// Localization keys for the centrally-seeded access rules (one rule per
    /// real API endpoint). Each rule has a display name and a description key so
    /// the Access Rules screen is fully localized just like every other seed.
    /// </summary>
    public static class AccessRulesSeed
    {
        public static class Home
        {
            public const string GetDashboardName = "AccessRules.Seed.Home.GetDashboard.Name";
            public const string GetDashboardDescription = "AccessRules.Seed.Home.GetDashboard.Description";
        }

        public static class User
        {
            public const string GetUsersName = "AccessRules.Seed.User.GetUsers.Name";
            public const string GetUsersDescription = "AccessRules.Seed.User.GetUsers.Description";
            public const string GetUserName = "AccessRules.Seed.User.GetUser.Name";
            public const string GetUserDescription = "AccessRules.Seed.User.GetUser.Description";
            public const string CreateUserName = "AccessRules.Seed.User.CreateUser.Name";
            public const string CreateUserDescription = "AccessRules.Seed.User.CreateUser.Description";
            public const string UpdateUserName = "AccessRules.Seed.User.UpdateUser.Name";
            public const string UpdateUserDescription = "AccessRules.Seed.User.UpdateUser.Description";
            public const string SetRolesName = "AccessRules.Seed.User.SetRoles.Name";
            public const string SetRolesDescription = "AccessRules.Seed.User.SetRoles.Description";
            public const string UpdatePasswordName = "AccessRules.Seed.User.UpdatePassword.Name";
            public const string UpdatePasswordDescription = "AccessRules.Seed.User.UpdatePassword.Description";
            public const string DeleteUserName = "AccessRules.Seed.User.DeleteUser.Name";
            public const string DeleteUserDescription = "AccessRules.Seed.User.DeleteUser.Description";
            public const string GetAdminPermissionHealthName = "AccessRules.Seed.User.GetAdminPermissionHealth.Name";
            public const string GetAdminPermissionHealthDescription = "AccessRules.Seed.User.GetAdminPermissionHealth.Description";
        }

        public static class Permission
        {
            public const string GetPermissionsName = "AccessRules.Seed.Permission.GetPermissions.Name";
            public const string GetPermissionsDescription = "AccessRules.Seed.Permission.GetPermissions.Description";
            public const string GetPermissionName = "AccessRules.Seed.Permission.GetPermission.Name";
            public const string GetPermissionDescription = "AccessRules.Seed.Permission.GetPermission.Description";
            public const string CreatePermissionName = "AccessRules.Seed.Permission.CreatePermission.Name";
            public const string CreatePermissionDescription = "AccessRules.Seed.Permission.CreatePermission.Description";
            public const string UpdatePermissionName = "AccessRules.Seed.Permission.UpdatePermission.Name";
            public const string UpdatePermissionDescription = "AccessRules.Seed.Permission.UpdatePermission.Description";
            public const string DeletePermissionName = "AccessRules.Seed.Permission.DeletePermission.Name";
            public const string DeletePermissionDescription = "AccessRules.Seed.Permission.DeletePermission.Description";
        }

        public static class Role
        {
            public const string GetRolesName = "AccessRules.Seed.Role.GetRoles.Name";
            public const string GetRolesDescription = "AccessRules.Seed.Role.GetRoles.Description";
            public const string GetRoleName = "AccessRules.Seed.Role.GetRole.Name";
            public const string GetRoleDescription = "AccessRules.Seed.Role.GetRole.Description";
            public const string CreateRoleName = "AccessRules.Seed.Role.CreateRole.Name";
            public const string CreateRoleDescription = "AccessRules.Seed.Role.CreateRole.Description";
            public const string UpdateRoleName = "AccessRules.Seed.Role.UpdateRole.Name";
            public const string UpdateRoleDescription = "AccessRules.Seed.Role.UpdateRole.Description";
            public const string DeleteRoleName = "AccessRules.Seed.Role.DeleteRole.Name";
            public const string DeleteRoleDescription = "AccessRules.Seed.Role.DeleteRole.Description";
            public const string GetRolePermissionsName = "AccessRules.Seed.Role.GetRolePermissions.Name";
            public const string GetRolePermissionsDescription = "AccessRules.Seed.Role.GetRolePermissions.Description";
            public const string SetRolePermissionsName = "AccessRules.Seed.Role.SetRolePermissions.Name";
            public const string SetRolePermissionsDescription = "AccessRules.Seed.Role.SetRolePermissions.Description";
            public const string GetRoleMenusName = "AccessRules.Seed.Role.GetRoleMenus.Name";
            public const string GetRoleMenusDescription = "AccessRules.Seed.Role.GetRoleMenus.Description";
            public const string SetRoleMenusName = "AccessRules.Seed.Role.SetRoleMenus.Name";
            public const string SetRoleMenusDescription = "AccessRules.Seed.Role.SetRoleMenus.Description";
        }

        public static class Menu
        {
            public const string GetMenusName = "AccessRules.Seed.Menu.GetMenus.Name";
            public const string GetMenusDescription = "AccessRules.Seed.Menu.GetMenus.Description";
            public const string GetMenuTreeName = "AccessRules.Seed.Menu.GetMenuTree.Name";
            public const string GetMenuTreeDescription = "AccessRules.Seed.Menu.GetMenuTree.Description";
            public const string GetMenuName = "AccessRules.Seed.Menu.GetMenu.Name";
            public const string GetMenuDescription = "AccessRules.Seed.Menu.GetMenu.Description";
            public const string CreateMenuName = "AccessRules.Seed.Menu.CreateMenu.Name";
            public const string CreateMenuDescription = "AccessRules.Seed.Menu.CreateMenu.Description";
            public const string UpdateMenuName = "AccessRules.Seed.Menu.UpdateMenu.Name";
            public const string UpdateMenuDescription = "AccessRules.Seed.Menu.UpdateMenu.Description";
            public const string DeleteMenuName = "AccessRules.Seed.Menu.DeleteMenu.Name";
            public const string DeleteMenuDescription = "AccessRules.Seed.Menu.DeleteMenu.Description";
            public const string GetMenuPermissionsName = "AccessRules.Seed.Menu.GetMenuPermissions.Name";
            public const string GetMenuPermissionsDescription = "AccessRules.Seed.Menu.GetMenuPermissions.Description";
            public const string SetMenuPermissionsName = "AccessRules.Seed.Menu.SetMenuPermissions.Name";
            public const string SetMenuPermissionsDescription = "AccessRules.Seed.Menu.SetMenuPermissions.Description";
        }

        public static class Localization
        {
            public const string GetAllName = "AccessRules.Seed.Localization.GetAll.Name";
            public const string GetAllDescription = "AccessRules.Seed.Localization.GetAll.Description";
            public const string GetByKeyName = "AccessRules.Seed.Localization.GetByKey.Name";
            public const string GetByKeyDescription = "AccessRules.Seed.Localization.GetByKey.Description";
            public const string UpsertName = "AccessRules.Seed.Localization.Upsert.Name";
            public const string UpsertDescription = "AccessRules.Seed.Localization.Upsert.Description";
            public const string DeleteName = "AccessRules.Seed.Localization.Delete.Name";
            public const string DeleteDescription = "AccessRules.Seed.Localization.Delete.Description";
        }

        public static class AccessRule
        {
            public const string GetAccessRulesName = "AccessRules.Seed.AccessRule.GetAccessRules.Name";
            public const string GetAccessRulesDescription = "AccessRules.Seed.AccessRule.GetAccessRules.Description";
            public const string GetAccessRuleName = "AccessRules.Seed.AccessRule.GetAccessRule.Name";
            public const string GetAccessRuleDescription = "AccessRules.Seed.AccessRule.GetAccessRule.Description";
            public const string CreateAccessRuleName = "AccessRules.Seed.AccessRule.CreateAccessRule.Name";
            public const string CreateAccessRuleDescription = "AccessRules.Seed.AccessRule.CreateAccessRule.Description";
            public const string UpdateAccessRuleName = "AccessRules.Seed.AccessRule.UpdateAccessRule.Name";
            public const string UpdateAccessRuleDescription = "AccessRules.Seed.AccessRule.UpdateAccessRule.Description";
            public const string DeleteAccessRuleName = "AccessRules.Seed.AccessRule.DeleteAccessRule.Name";
            public const string DeleteAccessRuleDescription = "AccessRules.Seed.AccessRule.DeleteAccessRule.Description";
            public const string GetAccessRulePermissionsName = "AccessRules.Seed.AccessRule.GetAccessRulePermissions.Name";
            public const string GetAccessRulePermissionsDescription = "AccessRules.Seed.AccessRule.GetAccessRulePermissions.Description";
            public const string SetAccessRulePermissionsName = "AccessRules.Seed.AccessRule.SetAccessRulePermissions.Name";
            public const string SetAccessRulePermissionsDescription = "AccessRules.Seed.AccessRule.SetAccessRulePermissions.Description";
            public const string GetRequiredPermissionsName = "AccessRules.Seed.AccessRule.GetRequiredPermissions.Name";
            public const string GetRequiredPermissionsDescription = "AccessRules.Seed.AccessRule.GetRequiredPermissions.Description";
        }
    }
}

