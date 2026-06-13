using Energy.Shared.Versioning;

namespace Energy.Web.Clients.Infrastructure;

internal static class ApiRoutes
{
    private static readonly string V1 = $"api/v{ApiVersions.V1UrlSegment}";

    public static class Auth
    {
        public static readonly string Login = $"{V1}/auth/login";
    }

    public static class Users
    {
        public static readonly string Base = $"{V1}/users";
        public static string ById(Guid id) => $"{Base}/{id}";
        public static string Password(Guid id) => $"{Base}/{id}/password";
        public static string ProfileImage(Guid id) => $"{Base}/{id}/profile-image";
        public static string Access(Guid id) => $"{Base}/{id}/access";
    }

    public static class Roles
    {
        public static readonly string Base = $"{V1}/roles";
        public static string ById(Guid id) => $"{Base}/{id}";
        public static string Permissions(Guid id) => $"{Base}/{id}/permissions";
    }

    public static class Permissions
    {
        public static readonly string Base = $"{V1}/permissions";
        public static string ByCode(string code) => $"{Base}/{Uri.EscapeDataString(code)}";
    }

    public static class Menus
    {
        public static readonly string Base = $"{V1}/menus";
        public static readonly string Me = $"{Base}/me";
        public static string ById(Guid id) => $"{Base}/{id}";
    }

    public static class ApiEndpoints
    {
        public static readonly string Base = $"{V1}/api-endpoints";
        public static string ById(Guid id) => $"{Base}/{id}";
    }

    public static class Localization
    {
        public static readonly string Base = $"{V1}/localization";
        public static string ByKey(string key) => $"{Base}/{Uri.EscapeDataString(key)}";
    }

    public static class Logs
    {
        public static readonly string Base = $"{V1}/audit-logs";
        public static string ById(long id) => $"{Base}/{id}";
    }

    public static class Home
    {
        public static readonly string Dashboard = $"{V1}/home/dashboard";
    }

    public static class Settings
    {
        public static readonly string Mine = $"{V1}/settings/me";
    }

    public static class Chat
    {
        public static readonly string Base = $"{V1}/chat";
        public static readonly string Contacts = $"{Base}/contacts";
        public static readonly string Messages = $"{Base}/messages";
        public static readonly string UnreadCount = $"{Base}/unread-count";
        public static string Conversation(Guid peerId) => $"{Base}/conversation/{peerId}";
        public static string MarkRead(Guid peerId) => $"{Base}/conversation/{peerId}/read";
        public static string MessageAttachment(Guid messageId) => $"{Base}/messages/{messageId}/attachment";
        public static string UserAvatar(Guid userId) => $"{Base}/users/{userId}/avatar";
        public static string MessageDelete(Guid messageId) => $"{Base}/messages/{messageId}";
        public static string MessageForward(Guid messageId) => $"{Base}/messages/{messageId}/forward";
        public static string MessageReact(Guid messageId) => $"{Base}/messages/{messageId}/react";

        // Groups
        public static readonly string Groups = $"{Base}/groups";
        public static readonly string GroupInvites = $"{Base}/groups/invites";
        public static string GroupInvite(Guid groupId) => $"{Base}/groups/{groupId}/invite";
        public static string GroupRespond(Guid groupId) => $"{Base}/groups/{groupId}/respond";
        public static string GroupMembers(Guid groupId) => $"{Base}/groups/{groupId}/members";
        public static string GroupMemberIds(Guid groupId) => $"{Base}/groups/{groupId}/member-ids";
        public static string GroupConversation(Guid groupId) => $"{Base}/groups/{groupId}/conversation";
        public static string GroupDelete(Guid groupId) => $"{Base}/groups/{groupId}";
        public static string GroupRemoveMember(Guid groupId, Guid userId) => $"{Base}/groups/{groupId}/members/{userId}";
        public static string GroupSetAdmin(Guid groupId, Guid userId) => $"{Base}/groups/{groupId}/members/{userId}/admin";
    }
}
