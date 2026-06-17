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


    /// <summary>
    /// <c>Energy.Shared.Identity.Permissions.PermissionCatalog</c> içindeki her
    /// yetki için görünen ad yerelleştirme anahtarları. Anahtar biçimi,
    /// <c>PermissionCatalog.BuildDisplayNameKey</c> ile aynıdır
    /// (<c>Permissions.{Module}.{Action}.Name</c>); böylece katalog kodu ile
    /// yerelleştirilmiş etiketi asla birbirinden ayrılmaz.
    /// </summary>
    public static class Permissions
    {
        public static class Dashboard
        {
            public const string ReadName = "Permissions.Dashboard.Read.Name";
        }

        public static class User
        {
            public const string ReadName = "Permissions.User.Read.Name";
            public const string ReadAllName = "Permissions.User.ReadAll.Name";
            public const string CreateName = "Permissions.User.Create.Name";
            public const string UpdateName = "Permissions.User.Update.Name";
            public const string DeleteName = "Permissions.User.Delete.Name";
        }

        public static class Role
        {
            public const string ReadName = "Permissions.Role.Read.Name";
            public const string ReadAllName = "Permissions.Role.ReadAll.Name";
            public const string CreateName = "Permissions.Role.Create.Name";
            public const string UpdateName = "Permissions.Role.Update.Name";
            public const string DeleteName = "Permissions.Role.Delete.Name";
        }

        public static class Permission
        {
            public const string ReadName = "Permissions.Permission.Read.Name";
            public const string ReadAllName = "Permissions.Permission.ReadAll.Name";
        }

        public static class Menu
        {
            public const string ReadName = "Permissions.Menu.Read.Name";
            public const string ReadAllName = "Permissions.Menu.ReadAll.Name";
            public const string CreateName = "Permissions.Menu.Create.Name";
            public const string UpdateName = "Permissions.Menu.Update.Name";
            public const string DeleteName = "Permissions.Menu.Delete.Name";
        }

        public static class ApiAccess
        {
            public const string ReadName = "Permissions.ApiAccess.Read.Name";
            public const string ReadAllName = "Permissions.ApiAccess.ReadAll.Name";
            public const string CreateName = "Permissions.ApiAccess.Create.Name";
            public const string UpdateName = "Permissions.ApiAccess.Update.Name";
            public const string DeleteName = "Permissions.ApiAccess.Delete.Name";
        }

        public static class Localization
        {
            public const string ReadName = "Permissions.Localization.Read.Name";
            public const string ReadAllName = "Permissions.Localization.ReadAll.Name";
            public const string CreateName = "Permissions.Localization.Create.Name";
            public const string UpdateName = "Permissions.Localization.Update.Name";
            public const string DeleteName = "Permissions.Localization.Delete.Name";
        }

        public static class Log
        {
            public const string ReadName = "Permissions.Log.Read.Name";
            public const string ReadAllName = "Permissions.Log.ReadAll.Name";
        }

        public static class Setting
        {
            public const string ReadName = "Permissions.Setting.Read.Name";
            public const string UpdateName = "Permissions.Setting.Update.Name";
        }

        public static class Profile
        {
            public const string ReadName = "Permissions.Profile.Read.Name";
            public const string UpdateName = "Permissions.Profile.Update.Name";
        }
    }

    /// <summary>
    /// Menü <c>NameKey</c> sabitleri. Bunlar, sistem tohumlayıcısının
    /// <c>Menu.NameKey</c> alanına yazdığı tam anahtarlardır ve menü ağacının gezinmeyi
    /// oluştururken paylaşılan kaynağa karşı çözümlediği aynı anahtarlardır.
    /// </summary>
    public static class Menus
    {
        public const string Dashboard = "Menus.Dashboard";
        public const string System = "Menus.System";
        public const string Profile = "Menus.Profile";
        public const string Chat = "Menus.Chat";
        public const string Settings = "Menus.Settings";
        public const string Notifications = "Menus.Notifications";
        public const string Users = "Menus.Users";
        public const string UserAccess = "Menus.UserAccess";
        public const string Roles = "Menus.Roles";
        public const string Permissions = "Menus.Permissions";
        public const string Menus_ = "Menus.Menus";
        public const string ApiEndpoints = "Menus.ApiEndpoints";
        public const string Localization = "Menus.Localization";
        public const string Logs = "Menus.Logs";
    }

    public static class Messages
    {
        public const string AdminUserCreationFailed = "Messages.AdminUserCreationFailed";
        public const string RoleNotFound = "Messages.RoleNotFound";
        public const string RoleAlreadyExists = "Messages.RoleAlreadyExists";
        public const string MenusNotFound = "Messages.MenusNotFound";
        public const string PermissionsNotFound = "Messages.PermissionsNotFound";
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

        // ---- New-architecture message keys ----
        public const string UserEmailAlreadyExists = "Messages.UserEmailAlreadyExists";
        public const string LogEntryNotFound = "Messages.LogEntryNotFound";
        public const string EndpointNotRegistered = "Messages.EndpointNotRegistered";
        public const string EndpointDisabled = "Messages.EndpointDisabled";
        public const string EndpointNotFound = "Messages.EndpointNotFound";
        public const string EndpointAlreadyExists = "Messages.EndpointAlreadyExists";
        public const string MissingPermission = "Messages.MissingPermission";
        public const string SystemRoleCannotBeRenamed = "Messages.SystemRoleCannotBeRenamed";
        public const string SystemRoleCannotBeDeleted = "Messages.SystemRoleCannotBeDeleted";
        public const string SuperAdminPermissionsAutoManaged = "Messages.SuperAdminPermissionsAutoManaged";
        public const string MenuParentCycle = "Messages.MenuParentCycle";
        public const string MenuHasChildren = "Messages.MenuHasChildren";
        public const string InvalidCredentials = "Messages.InvalidCredentials";
        public const string TokenNoLongerValid = "Messages.TokenNoLongerValid";
    }

    /// <summary>
    /// Merkezi olarak seed edilen rol kataloğu için yerelleştirme anahtarları.
    /// Veritabanı bu ANAHTARLARI <c>Role.Description</c> alanında saklar; değerler
    /// okuma anında paylaşılan kaynaktan çözümlenir.
    /// </summary>
    public static class RoleSeed
    {
        public const string SuperAdminDescription = "RoleSeed.SuperAdmin.Description";
        public const string SystemAdminDescription = "RoleSeed.SystemAdmin.Description";
        public const string OperationsManagerDescription = "RoleSeed.OperationsManager.Description";
        public const string SecurityAuditorDescription = "RoleSeed.SecurityAuditor.Description";
        public const string LocalizationEditorDescription = "RoleSeed.LocalizationEditor.Description";
        public const string ReadOnlyViewerDescription = "RoleSeed.ReadOnlyViewer.Description";
        public const string BasicUserDescription = "RoleSeed.BasicUser.Description";
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
        public const string BusinessOverview = "Dashboard.BusinessOverview";
        public const string BusinessOverviewDescription = "Dashboard.BusinessOverviewDescription";
        public const string Brand = "Dashboard.Brand";
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
        public const string DevQuickLogin = "Auth.DevQuickLogin";
        public const string DevSelectUser = "Auth.DevSelectUser";
        public const string MyProfile = "Auth.MyProfile";
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
        public const string MenuSearch = "Layout.MenuSearch";
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
        public const string ConfirmDelete = "ProfileScreen.ConfirmDelete";
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

    /// <summary>İş kuralı (modül) satır eylem butonları için anahtarlar.</summary>
    public static class ModuleActions
    {
        public const string Column = "ModuleActions.Column";
        public const string Approve = "ModuleActions.Approve";
        public const string Reject = "ModuleActions.Reject";
        public const string Return = "ModuleActions.Return";
        public const string Cancel = "ModuleActions.Cancel";
        public const string Reverse = "ModuleActions.Reverse";
        public const string Receive = "ModuleActions.Receive";
        public const string Close = "ModuleActions.Close";
        public const string Reopen = "ModuleActions.Reopen";
        public const string Activate = "ModuleActions.Activate";
        public const string Validate = "ModuleActions.Validate";
        public const string ConfirmTitle = "ModuleActions.ConfirmTitle";
        public const string ConfirmMessage = "ModuleActions.ConfirmMessage";
        public const string NotePrompt = "ModuleActions.NotePrompt";
        public const string Succeeded = "ModuleActions.Succeeded";
        public const string ValidationOk = "ModuleActions.ValidationOk";
        public const string ValidationIssues = "ModuleActions.ValidationIssues";
    }

    /// <summary>Ana-detay (master-detail) alt-koleksiyon sekme başlıkları için anahtarlar.</summary>
    public static class ModuleDetails
    {
        public const string RequestLines = "ModuleDetails.RequestLines";
        public const string PurchaseOrderLines = "ModuleDetails.PurchaseOrderLines";
        public const string WorkOrderAssignments = "ModuleDetails.WorkOrderAssignments";
        public const string WorkOrderMaterialPlans = "ModuleDetails.WorkOrderMaterialPlans";
        public const string WorkOrderChecklists = "ModuleDetails.WorkOrderChecklists";
        public const string WorkOrderStatusHistories = "ModuleDetails.WorkOrderStatusHistories";
        public const string DailySiteReportWorkers = "ModuleDetails.DailySiteReportWorkers";
        public const string DailySiteReportEquipments = "ModuleDetails.DailySiteReportEquipments";
        public const string DailySiteReportMaterials = "ModuleDetails.DailySiteReportMaterials";
        public const string TimesheetLines = "ModuleDetails.TimesheetLines";
        public const string EquipmentAssignments = "ModuleDetails.EquipmentAssignments";
        public const string EquipmentMaintenances = "ModuleDetails.EquipmentMaintenances";
        public const string FinancialTransactionLines = "ModuleDetails.FinancialTransactionLines";
        public const string BudgetLines = "ModuleDetails.BudgetLines";
        public const string ContractLines = "ModuleDetails.ContractLines";
        public const string ContractParties = "ModuleDetails.ContractParties";
        public const string ContractAmendments = "ModuleDetails.ContractAmendments";
        public const string ProgressPaymentLines = "ModuleDetails.ProgressPaymentLines";
        public const string ProgressPaymentDeductions = "ModuleDetails.ProgressPaymentDeductions";
        public const string MaterialAttributeValues = "ModuleDetails.MaterialAttributeValues";
        public const string MaterialUnitConversions = "ModuleDetails.MaterialUnitConversions";
        public const string WarehouseLocations = "ModuleDetails.WarehouseLocations";
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

    public static class Alerts
    {
        public const string Success = "Alerts.Success";
        public const string Information = "Alerts.Information";
        public const string Warning = "Alerts.Warning";
        public const string Error = "Alerts.Error";
        public const string Confirm = "Alerts.Confirm";
        public const string Ok = "Alerts.Ok";
    }

    public static class Screen
    {
        public const string EntityBadge = "Screen.EntityBadge";
        public const string EntityBadgeTitle = "Screen.EntityBadgeTitle";
        public const string ProcessBadge = "Screen.ProcessBadge";
        public const string ProcessBadgeTitle = "Screen.ProcessBadgeTitle";
        public const string ReportBadge = "Screen.ReportBadge";
        public const string ReportBadgeTitle = "Screen.ReportBadgeTitle";
        public const string LookupMissing = "Screen.LookupMissing";
    }

    public static class Reports
    {
        public const string StartDate = "Reports.StartDate";
        public const string EndDate = "Reports.EndDate";
        public const string Status = "Reports.Status";
        public const string Export = "Reports.Export";
        public const string Refresh = "Reports.Refresh";
        public const string Filters = "Reports.Filters";
        public const string AllStatuses = "Reports.AllStatuses";
    }

    public static class Processes
    {
        public const string Submit = "Processes.Submit";
        public const string Reset = "Processes.Reset";
        public const string GenericError = "Processes.GenericError";
        public const string GenericSuccess = "Processes.GenericSuccess";
        public const string ResultTotalCost = "Processes.ResultTotalCost";
        public const string ResultAllocations = "Processes.ResultAllocations";
        public const string ResultTransaction = "Processes.ResultTransaction";
        public const string ResultLines = "Processes.ResultLines";
        public const string ResultTotal = "Processes.ResultTotal";
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

    public static class UserAccessScreen
    {
        public const string Title = "UserAccessScreen.Title";
        public const string Subtitle = "UserAccessScreen.Subtitle";
        public const string SelectUserPrompt = "UserAccessScreen.SelectUserPrompt";
        public const string RolesTab = "UserAccessScreen.RolesTab";
        public const string PermissionsTab = "UserAccessScreen.PermissionsTab";
        public const string RolesNote = "UserAccessScreen.RolesNote";
        public const string PermissionsNote = "UserAccessScreen.PermissionsNote";
        public const string Inherited = "UserAccessScreen.Inherited";
        public const string Saved = "UserAccessScreen.Saved";
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
        public const string PermissionsTitle = "RolesScreen.PermissionsTitle";
    }

    public static class PermissionsScreen
    {
        public const string Title = "PermissionsScreen.Title";
        public const string Subtitle = "PermissionsScreen.Subtitle";
        public const string Code = "PermissionsScreen.Code";
        public const string Name = "PermissionsScreen.Name";
        public const string Module = "PermissionsScreen.Module";
        public const string Action = "PermissionsScreen.Action";
        public const string RoleCount = "PermissionsScreen.RoleCount";
        public const string MenuCount = "PermissionsScreen.MenuCount";
        public const string EndpointCount = "PermissionsScreen.EndpointCount";
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

    public static class ApiEndpointsScreen
    {
        public const string Title = "ApiEndpointsScreen.Title";
        public const string Subtitle = "ApiEndpointsScreen.Subtitle";
        public const string Name = "ApiEndpointsScreen.Name";
        public const string HttpMethod = "ApiEndpointsScreen.HttpMethod";
        public const string Path = "ApiEndpointsScreen.Path";
        public const string Description = "ApiEndpointsScreen.Description";
        public const string RequiredPermission = "ApiEndpointsScreen.RequiredPermission";
        public const string IsActive = "ApiEndpointsScreen.IsActive";
        public const string PopupTitle = "ApiEndpointsScreen.PopupTitle";
    }

    public static class SettingsScreen
    {
        public const string Title = "SettingsScreen.Title";
        public const string Subtitle = "SettingsScreen.Subtitle";
        public const string Notifications = "SettingsScreen.Notifications";
        public const string NotificationSound = "SettingsScreen.NotificationSound";
        public const string CallSound = "SettingsScreen.CallSound";
        public const string DesktopNotifications = "SettingsScreen.DesktopNotifications";
        public const string Privacy = "SettingsScreen.Privacy";
        public const string ReadReceipts = "SettingsScreen.ReadReceipts";
        public const string Appearance = "SettingsScreen.Appearance";
        public const string Theme = "SettingsScreen.Theme";
        public const string ThemeSystem = "SettingsScreen.ThemeSystem";
        public const string ThemeLight = "SettingsScreen.ThemeLight";
        public const string ThemeDark = "SettingsScreen.ThemeDark";
        public const string Save = "SettingsScreen.Save";
        public const string Saved = "SettingsScreen.Saved";
    }

    public static class ChatScreen
    {
        public const string Title = "ChatScreen.Title";
        public const string Subtitle = "ChatScreen.Subtitle";
        public const string SearchContacts = "ChatScreen.SearchContacts";
        public const string SelectContact = "ChatScreen.SelectContact";
        public const string MessagePlaceholder = "ChatScreen.MessagePlaceholder";
        public const string Notifications = "ChatScreen.Notifications";
        public const string NoNotifications = "ChatScreen.NoNotifications";
        public const string NewMessageFrom = "ChatScreen.NewMessageFrom";
        public const string AttachFile = "ChatScreen.AttachFile";
        public const string AttachmentLabel = "ChatScreen.AttachmentLabel";
        public const string Online = "ChatScreen.Online";
        public const string Offline = "ChatScreen.Offline";
        public const string Typing = "ChatScreen.Typing";
        public const string ConnectionStatus = "ChatScreen.ConnectionStatus";
        public const string Connected = "ChatScreen.Connected";
        public const string Connecting = "ChatScreen.Connecting";
        public const string Reconnecting = "ChatScreen.Reconnecting";
        public const string Disconnected = "ChatScreen.Disconnected";
        public const string OnlineUsers = "ChatScreen.OnlineUsers";
        public const string GroupName = "ChatScreen.GroupName";
        public const string CreateGroup = "ChatScreen.CreateGroup";
        public const string InviteToGroup = "ChatScreen.InviteToGroup";
        public const string DeleteGroup = "ChatScreen.DeleteGroup";
        public const string Calling = "ChatScreen.Calling";
        public const string VoiceUnsupported = "ChatScreen.VoiceUnsupported";
        public const string VoiceDenied = "ChatScreen.VoiceDenied";
        public const string GroupInvite = "ChatScreen.GroupInvite";
        public const string Group = "ChatScreen.Group";
        // Yeni grup / üyeler / iletim popup'ları
        public const string NewGroup = "ChatScreen.NewGroup";
        public const string Members = "ChatScreen.Members";
        public const string AddMember = "ChatScreen.AddMember";
        public const string GroupMembers = "ChatScreen.GroupMembers";
        public const string GroupNameRequired = "ChatScreen.GroupNameRequired";
        public const string GroupCreated = "ChatScreen.GroupCreated";
        public const string GroupDeleted = "ChatScreen.GroupDeleted";
        public const string InviteSent = "ChatScreen.InviteSent";
        public const string MessageForwarded = "ChatScreen.MessageForwarded";
        public const string MessageActionReact = "ChatScreen.MessageActionReact";
        public const string MessageActionReply = "ChatScreen.MessageActionReply";
        public const string MessageActionForward = "ChatScreen.MessageActionForward";
        public const string MessageActionDelete = "ChatScreen.MessageActionDelete";
        public const string ConfirmDeleteMessage = "ChatScreen.ConfirmDeleteMessage";
        public const string ConfirmRemoveMember = "ChatScreen.ConfirmRemoveMember";
        public const string ConfirmDeleteGroup = "ChatScreen.ConfirmDeleteGroup";
        public const string MakeAdmin = "ChatScreen.MakeAdmin";
        public const string RemoveAdmin = "ChatScreen.RemoveAdmin";
        public const string RemoveFromGroup = "ChatScreen.RemoveFromGroup";
        public const string RoleOwner = "ChatScreen.RoleOwner";
        public const string RoleAdmin = "ChatScreen.RoleAdmin";
        public const string RolePending = "ChatScreen.RolePending";
        public const string RoleMember = "ChatScreen.RoleMember";
        public const string ForwardTitle = "ChatScreen.ForwardTitle";
        public const string Contacts = "ChatScreen.Contacts";
        public const string Groups = "ChatScreen.Groups";
        public const string RecentChats = "ChatScreen.RecentChats";
        public const string AcceptCall = "ChatScreen.AcceptCall";
        public const string RejectCall = "ChatScreen.RejectCall";
        public const string CallConnected = "ChatScreen.CallConnected";
        public const string CallNeedsOnline = "ChatScreen.CallNeedsOnline";
        public const string MicNotAccessible = "ChatScreen.MicNotAccessible";
        public const string GenericError = "ChatScreen.GenericError";
        // Görünüm rozetleri / aria-label / boş veri metinleri
        public const string ToggleChats = "ChatScreen.ToggleChats";
        public const string ToggleContacts = "ChatScreen.ToggleContacts";
        public const string CloseSidebar = "ChatScreen.CloseSidebar";
        public const string WelcomeMessage = "ChatScreen.WelcomeMessage";
        public const string VoiceCall = "ChatScreen.VoiceCall";
        public const string VoiceMessage = "ChatScreen.VoiceMessage";
        public const string SendVoice = "ChatScreen.SendVoice";
        public const string CancelLabel = "ChatScreen.CancelLabel";
        public const string NoGroups = "ChatScreen.NoGroups";
    }

    public static class LogsScreen
    {
        public const string Title = "LogsScreen.Title";
        public const string Subtitle = "LogsScreen.Subtitle";
        public const string DetailTitle = "LogsScreen.DetailTitle";
        public const string Occurred = "LogsScreen.Occurred";
        public const string Source = "LogsScreen.Source";
        public const string User = "LogsScreen.User";
        public const string IpAddress = "LogsScreen.IpAddress";
        public const string HttpMethod = "LogsScreen.HttpMethod";
        public const string Path = "LogsScreen.Path";
        public const string StatusCode = "LogsScreen.StatusCode";
        public const string IsSuccess = "LogsScreen.IsSuccess";
        public const string HasException = "LogsScreen.HasException";
        public const string DurationMs = "LogsScreen.DurationMs";
        public const string Details = "LogsScreen.Details";
        public const string NotFound = "LogsScreen.NotFound";
        public const string Id = "LogsScreen.Id";
        public const string QueryString = "LogsScreen.QueryString";
        public const string ExceptionType = "LogsScreen.ExceptionType";
        public const string ExceptionMessage = "LogsScreen.ExceptionMessage";
        public const string CorrelationId = "LogsScreen.CorrelationId";
        public const string RequestBody = "LogsScreen.RequestBody";
        public const string ResponseBody = "LogsScreen.ResponseBody";
    }
}

