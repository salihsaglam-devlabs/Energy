using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Energy.Infrastructure.Seeding;

/// <summary>
/// <see cref="SystemSeeder"/> için geçiş (migration) içermeyen şema "tamamlamaları".
/// Bunlar, ana tohumlayıcı dosyasını okunabilir, yukarıdan aşağıya akan VERİ akışına
/// (yetkiler → roller → rol-yetkileri → menüler/uç noktalar → kullanıcılar) odaklı
/// tutar; mevcut veritabanlarını güncel modele taşıyan idempotent DDL ise burada yer
/// alır. Her metot hem PostgreSQL hem de SQL Server üzerinde güvenle yeniden çalıştırılabilir.
/// </summary>
public sealed partial class SystemSeeder
{
    /// <summary>
    /// Tüm şema işlemleri için tek giriş noktası. SQL Server'da şemanın tamamını
    /// modelden oluşturur (EnsureCreated); PostgreSQL'de geçiş içermeyen sütun/tablo
    /// tamamlamalarını uygular. İlk şemadan sonra eklenen çapraz kesen tablolar
    /// (sohbet grupları/ekleri, kullanıcı bazlı ayarlar) HER İKİ sağlayıcıda da
    /// sağlanır; çünkü EnsureCreated, zaten var olan bir SQL Server veritabanına yeni
    /// tablolar eklemez. Her adım idempotenttir.
    /// </summary>
    private async Task EnsureSchemaAsync(CancellationToken ct)
    {
        if (_db.Database.IsSqlServer())
        {
            _logger.LogInformation("Seeding: ensuring SQL Server schema (EnsureCreated)");
            await _db.Database.EnsureCreatedAsync(ct);
        }
        else
        {
            _logger.LogInformation("Seeding: audit log schema (request/response columns)");
            await EnsureAuditSchemaAsync(ct);

            _logger.LogInformation("Seeding: direct user-permission table");
            await EnsureUserPermissionSchemaAsync(ct);

            _logger.LogInformation("Seeding: profile-image columns");
            await EnsureProfileImageSchemaAsync(ct);

            _logger.LogInformation("Seeding: chat message table");
            await EnsureChatSchemaAsync(ct);
        }

        _logger.LogInformation("Seeding: chat group tables + message GroupId column");
        await EnsureChatGroupSchemaAsync(ct);

        _logger.LogInformation("Seeding: chat reply column + reactions table");
        await EnsureChatExtrasSchemaAsync(ct);

        _logger.LogInformation("Seeding: per-user settings table");
        await EnsureUserSettingsSchemaAsync(ct);
    }

    /// <summary>
    /// İstek/yanıt denetim sütunlarını idempotent şekilde ekler. Projenin geçiş
    /// geçmişi yoktur; bu nedenle bu, herhangi bir denetim eklemesi çalışmadan önce
    /// mevcut veritabanlarının yeni sütunları kazanmasını garanti eder. Yeni
    /// veritabanlarında güvenli ve işlemsizdir (no-op).
    /// </summary>
    private async Task EnsureAuditSchemaAsync(CancellationToken ct)
    {
        const string sql = """
            ALTER TABLE "AuditLogs" ADD COLUMN IF NOT EXISTS "QueryString" character varying(2000);
            ALTER TABLE "AuditLogs" ADD COLUMN IF NOT EXISTS "Source" character varying(10);
            ALTER TABLE "AuditLogs" ADD COLUMN IF NOT EXISTS "RequestBody" text;
            ALTER TABLE "AuditLogs" ADD COLUMN IF NOT EXISTS "ResponseBody" text;
            """;
        try
        {
            await _db.Database.ExecuteSqlRawAsync(sql, ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not ensure AuditLogs request/response columns; they may already exist or the table is not yet created.");
        }
    }

    /// <summary>
    /// Doğrudan, kullanıcı bazlı yetki atamalarını (Kullanıcı Erişimi ekranından
    /// yönetilir) destekleyen <c>UserPermissions</c> tablosunu idempotent şekilde
    /// oluşturur. Denetim sütunları için kullanılan geçiş içermeyen yaklaşımı yansıtır.
    /// </summary>
    private async Task EnsureUserPermissionSchemaAsync(CancellationToken ct)
    {
        const string sql = """
            CREATE TABLE IF NOT EXISTS "UserPermissions" (
                "UserId" uuid NOT NULL,
                "PermissionCode" character varying(150) NOT NULL,
                CONSTRAINT "PK_UserPermissions" PRIMARY KEY ("UserId", "PermissionCode"),
                CONSTRAINT "FK_UserPermissions_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE,
                CONSTRAINT "FK_UserPermissions_Permissions_PermissionCode" FOREIGN KEY ("PermissionCode") REFERENCES "Permissions" ("Code") ON DELETE RESTRICT
            );
            CREATE INDEX IF NOT EXISTS "IX_UserPermissions_PermissionCode" ON "UserPermissions" ("PermissionCode");
            """;
        try
        {
            await _db.Database.ExecuteSqlRawAsync(sql, ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not ensure the UserPermissions table; it may already exist or a referenced table is not yet created.");
        }
    }

    /// <summary>
    /// İkili profil resmi sütunlarını <c>Users</c> tablosuna idempotent şekilde ekler.
    /// Zaten varsa güvenli ve işlemsizdir (geçiş içermeyen konvansiyon).
    /// </summary>
    private async Task EnsureProfileImageSchemaAsync(CancellationToken ct)
    {
        const string sql = """
            ALTER TABLE "Users" ADD COLUMN IF NOT EXISTS "ProfileImage" bytea;
            ALTER TABLE "Users" ADD COLUMN IF NOT EXISTS "ProfileImageContentType" character varying(100);
            """;
        try
        {
            await _db.Database.ExecuteSqlRawAsync(sql, ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not ensure the Users profile-image columns; they may already exist.");
        }
    }

    /// <summary>
    /// Doğrudan mesajlaşma özelliğini destekleyen <c>ChatMessages</c> tablosunu
    /// idempotent şekilde oluşturur. Geçiş içermeyen DDL yaklaşımını yansıtır.
    /// </summary>
    private async Task EnsureChatSchemaAsync(CancellationToken ct)
    {
        const string sql = """
            CREATE TABLE IF NOT EXISTS "ChatMessages" (
                "Id" uuid NOT NULL,
                "SenderId" uuid NOT NULL,
                "RecipientId" uuid NOT NULL,
                "Text" character varying(4000) NOT NULL,
                "IsRead" boolean NOT NULL DEFAULT FALSE,
                "ReadAt" timestamp with time zone,
                "CreatedAt" timestamp with time zone NOT NULL,
                "CreatedBy" uuid,
                "UpdatedAt" timestamp with time zone,
                "UpdatedBy" uuid,
                "IsDeleted" boolean NOT NULL DEFAULT FALSE,
                "DeletedAt" timestamp with time zone,
                "DeletedBy" uuid,
                CONSTRAINT "PK_ChatMessages" PRIMARY KEY ("Id"),
                CONSTRAINT "FK_ChatMessages_Users_SenderId" FOREIGN KEY ("SenderId") REFERENCES "Users" ("Id") ON DELETE CASCADE,
                CONSTRAINT "FK_ChatMessages_Users_RecipientId" FOREIGN KEY ("RecipientId") REFERENCES "Users" ("Id") ON DELETE CASCADE
            );
            CREATE INDEX IF NOT EXISTS "IX_ChatMessages_SenderId_RecipientId" ON "ChatMessages" ("SenderId", "RecipientId");
            CREATE INDEX IF NOT EXISTS "IX_ChatMessages_RecipientId_IsRead" ON "ChatMessages" ("RecipientId", "IsRead");
            """;
        try
        {
            await _db.Database.ExecuteSqlRawAsync(sql, ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not ensure the ChatMessages table; it may already exist or a referenced table is not yet created.");
        }
    }

    /// <summary>
    /// <c>ChatGroups</c> / <c>ChatGroupMembers</c> tablolarını (<c>IsAdmin</c> sütunu
    /// dahil) ve <c>ChatMessages.GroupId</c> sütununu idempotent şekilde oluşturur;
    /// ayrıca <c>ChatMessages.RecipientId</c> sütununu NULL kabul edecek şekilde
    /// gevşetir (grup mesajlarının tek bir alıcısı yoktur). Sağlayıcıya özgü ancak idempotenttir.
    /// </summary>
    private async Task EnsureChatGroupSchemaAsync(CancellationToken ct)
    {
        var sql = _db.Database.IsSqlServer()
            ? """
              IF OBJECT_ID(N'[ChatGroups]', N'U') IS NULL
              CREATE TABLE [ChatGroups] (
                  [Id] uniqueidentifier NOT NULL,
                  [Name] nvarchar(150) NOT NULL,
                  [OwnerId] uniqueidentifier NOT NULL,
                  [CreatedAt] datetime2 NOT NULL,
                  [CreatedBy] uniqueidentifier NULL,
                  [UpdatedAt] datetime2 NULL,
                  [UpdatedBy] uniqueidentifier NULL,
                  [IsDeleted] bit NOT NULL CONSTRAINT [DF_ChatGroups_IsDeleted] DEFAULT(0),
                  [DeletedAt] datetime2 NULL,
                  [DeletedBy] uniqueidentifier NULL,
                  CONSTRAINT [PK_ChatGroups] PRIMARY KEY ([Id])
              );
              IF OBJECT_ID(N'[ChatGroupMembers]', N'U') IS NULL
              CREATE TABLE [ChatGroupMembers] (
                  [Id] uniqueidentifier NOT NULL,
                  [GroupId] uniqueidentifier NOT NULL,
                  [UserId] uniqueidentifier NOT NULL,
                  [Status] int NOT NULL,
                  [IsOwner] bit NOT NULL CONSTRAINT [DF_ChatGroupMembers_IsOwner] DEFAULT(0),
                  [InvitedById] uniqueidentifier NULL,
                  [CreatedAt] datetime2 NOT NULL,
                  [CreatedBy] uniqueidentifier NULL,
                  [UpdatedAt] datetime2 NULL,
                  [UpdatedBy] uniqueidentifier NULL,
                  [IsDeleted] bit NOT NULL CONSTRAINT [DF_ChatGroupMembers_IsDeleted] DEFAULT(0),
                  [DeletedAt] datetime2 NULL,
                  [DeletedBy] uniqueidentifier NULL,
                  CONSTRAINT [PK_ChatGroupMembers] PRIMARY KEY ([Id]),
                  CONSTRAINT [FK_ChatGroupMembers_ChatGroups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [ChatGroups]([Id]) ON DELETE CASCADE
              );
              IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ChatGroupMembers_GroupId_UserId')
              CREATE UNIQUE INDEX [IX_ChatGroupMembers_GroupId_UserId] ON [ChatGroupMembers]([GroupId],[UserId]);
              IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ChatGroupMembers_UserId_Status')
              CREATE INDEX [IX_ChatGroupMembers_UserId_Status] ON [ChatGroupMembers]([UserId],[Status]);
              IF COL_LENGTH('ChatGroupMembers','IsAdmin') IS NULL ALTER TABLE [ChatGroupMembers] ADD [IsAdmin] bit NOT NULL CONSTRAINT [DF_ChatGroupMembers_IsAdmin] DEFAULT(0);
              IF COL_LENGTH('ChatMessages','GroupId') IS NULL ALTER TABLE [ChatMessages] ADD [GroupId] uniqueidentifier NULL;
              IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ChatMessages_GroupId')
              CREATE INDEX [IX_ChatMessages_GroupId] ON [ChatMessages]([GroupId]);
              IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('ChatMessages') AND name = 'RecipientId' AND is_nullable = 0)
              BEGIN
                  IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ChatMessages_SenderId_RecipientId') DROP INDEX [IX_ChatMessages_SenderId_RecipientId] ON [ChatMessages];
                  IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ChatMessages_RecipientId_IsRead') DROP INDEX [IX_ChatMessages_RecipientId_IsRead] ON [ChatMessages];
                  ALTER TABLE [ChatMessages] ALTER COLUMN [RecipientId] uniqueidentifier NULL;
                  CREATE INDEX [IX_ChatMessages_SenderId_RecipientId] ON [ChatMessages]([SenderId],[RecipientId]);
                  CREATE INDEX [IX_ChatMessages_RecipientId_IsRead] ON [ChatMessages]([RecipientId],[IsRead]);
              END
              """
            : """
              CREATE TABLE IF NOT EXISTS "ChatGroups" (
                  "Id" uuid NOT NULL,
                  "Name" character varying(150) NOT NULL,
                  "OwnerId" uuid NOT NULL,
                  "CreatedAt" timestamp with time zone NOT NULL,
                  "CreatedBy" uuid,
                  "UpdatedAt" timestamp with time zone,
                  "UpdatedBy" uuid,
                  "IsDeleted" boolean NOT NULL DEFAULT FALSE,
                  "DeletedAt" timestamp with time zone,
                  "DeletedBy" uuid,
                  CONSTRAINT "PK_ChatGroups" PRIMARY KEY ("Id")
              );
              CREATE TABLE IF NOT EXISTS "ChatGroupMembers" (
                  "Id" uuid NOT NULL,
                  "GroupId" uuid NOT NULL,
                  "UserId" uuid NOT NULL,
                  "Status" integer NOT NULL,
                  "IsOwner" boolean NOT NULL DEFAULT FALSE,
                  "InvitedById" uuid,
                  "CreatedAt" timestamp with time zone NOT NULL,
                  "CreatedBy" uuid,
                  "UpdatedAt" timestamp with time zone,
                  "UpdatedBy" uuid,
                  "IsDeleted" boolean NOT NULL DEFAULT FALSE,
                  "DeletedAt" timestamp with time zone,
                  "DeletedBy" uuid,
                  CONSTRAINT "PK_ChatGroupMembers" PRIMARY KEY ("Id"),
                  CONSTRAINT "FK_ChatGroupMembers_ChatGroups_GroupId" FOREIGN KEY ("GroupId") REFERENCES "ChatGroups" ("Id") ON DELETE CASCADE
              );
              CREATE UNIQUE INDEX IF NOT EXISTS "IX_ChatGroupMembers_GroupId_UserId" ON "ChatGroupMembers" ("GroupId","UserId");
              CREATE INDEX IF NOT EXISTS "IX_ChatGroupMembers_UserId_Status" ON "ChatGroupMembers" ("UserId","Status");
              ALTER TABLE "ChatGroupMembers" ADD COLUMN IF NOT EXISTS "IsAdmin" boolean NOT NULL DEFAULT FALSE;
              ALTER TABLE "ChatMessages" ADD COLUMN IF NOT EXISTS "GroupId" uuid;
              CREATE INDEX IF NOT EXISTS "IX_ChatMessages_GroupId" ON "ChatMessages" ("GroupId");
              ALTER TABLE "ChatMessages" ALTER COLUMN "RecipientId" DROP NOT NULL;
              """;
        try
        {
            await _db.Database.ExecuteSqlRawAsync(sql, ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not ensure the chat group schema; parts may already exist.");
        }
    }

    /// <summary>
    /// <c>ChatMessages.ReplyToMessageId</c> sütununu idempotent şekilde ekler ve
    /// <c>ChatMessageReactions</c> tablosunu (emoji tepkileri) oluşturur. Her iki
    /// sağlayıcıda idempotenttir.
    /// </summary>
    private async Task EnsureChatExtrasSchemaAsync(CancellationToken ct)
    {
        var sql = _db.Database.IsSqlServer()
            ? """
              IF COL_LENGTH('ChatMessages','ReplyToMessageId') IS NULL ALTER TABLE [ChatMessages] ADD [ReplyToMessageId] uniqueidentifier NULL;
              IF OBJECT_ID(N'[ChatMessageReactions]', N'U') IS NULL
              CREATE TABLE [ChatMessageReactions] (
                  [Id] uniqueidentifier NOT NULL,
                  [MessageId] uniqueidentifier NOT NULL,
                  [UserId] uniqueidentifier NOT NULL,
                  [Emoji] nvarchar(16) NOT NULL,
                  [CreatedAt] datetime2 NOT NULL,
                  [CreatedBy] uniqueidentifier NULL,
                  [UpdatedAt] datetime2 NULL,
                  [UpdatedBy] uniqueidentifier NULL,
                  [IsDeleted] bit NOT NULL CONSTRAINT [DF_ChatMessageReactions_IsDeleted] DEFAULT(0),
                  [DeletedAt] datetime2 NULL,
                  [DeletedBy] uniqueidentifier NULL,
                  CONSTRAINT [PK_ChatMessageReactions] PRIMARY KEY ([Id]),
                  CONSTRAINT [FK_ChatMessageReactions_ChatMessages_MessageId] FOREIGN KEY ([MessageId]) REFERENCES [ChatMessages]([Id]) ON DELETE CASCADE
              );
              IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ChatMessageReactions_MessageId_UserId')
              CREATE UNIQUE INDEX [IX_ChatMessageReactions_MessageId_UserId] ON [ChatMessageReactions]([MessageId],[UserId]);
              """
            : """
              ALTER TABLE "ChatMessages" ADD COLUMN IF NOT EXISTS "ReplyToMessageId" uuid;
              CREATE TABLE IF NOT EXISTS "ChatMessageReactions" (
                  "Id" uuid NOT NULL,
                  "MessageId" uuid NOT NULL,
                  "UserId" uuid NOT NULL,
                  "Emoji" character varying(16) NOT NULL,
                  "CreatedAt" timestamp with time zone NOT NULL,
                  "CreatedBy" uuid,
                  "UpdatedAt" timestamp with time zone,
                  "UpdatedBy" uuid,
                  "IsDeleted" boolean NOT NULL DEFAULT FALSE,
                  "DeletedAt" timestamp with time zone,
                  "DeletedBy" uuid,
                  CONSTRAINT "PK_ChatMessageReactions" PRIMARY KEY ("Id"),
                  CONSTRAINT "FK_ChatMessageReactions_ChatMessages_MessageId" FOREIGN KEY ("MessageId") REFERENCES "ChatMessages" ("Id") ON DELETE CASCADE
              );
              CREATE UNIQUE INDEX IF NOT EXISTS "IX_ChatMessageReactions_MessageId_UserId" ON "ChatMessageReactions" ("MessageId","UserId");
              """;
        try
        {
            await _db.Database.ExecuteSqlRawAsync(sql, ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not ensure the chat extras schema; parts may already exist.");
        }
    }

    /// <summary>
    /// Kullanıcı bazlı tercihleri (bildirim sesi, tema, okundu bilgileri, ...) destekleyen
    /// <c>UserSettings</c> tablosunu idempotent şekilde oluşturur. Her iki sağlayıcıda
    /// idempotenttir (geçiş içermeyen konvansiyon).
    /// </summary>
    private async Task EnsureUserSettingsSchemaAsync(CancellationToken ct)
    {
        var sql = _db.Database.IsSqlServer()
            ? """
              IF OBJECT_ID(N'[UserSettings]', N'U') IS NULL
              CREATE TABLE [UserSettings] (
                  [UserId] uniqueidentifier NOT NULL,
                  [NotificationSound] bit NOT NULL CONSTRAINT [DF_UserSettings_NotificationSound] DEFAULT(1),
                  [CallSound] bit NOT NULL CONSTRAINT [DF_UserSettings_CallSound] DEFAULT(1),
                  [DesktopNotifications] bit NOT NULL CONSTRAINT [DF_UserSettings_DesktopNotifications] DEFAULT(1),
                  [ReadReceipts] bit NOT NULL CONSTRAINT [DF_UserSettings_ReadReceipts] DEFAULT(1),
                  [Theme] nvarchar(20) NOT NULL CONSTRAINT [DF_UserSettings_Theme] DEFAULT(N'system'),
                  [UpdatedAt] datetime2 NULL,
                  CONSTRAINT [PK_UserSettings] PRIMARY KEY ([UserId]),
                  CONSTRAINT [FK_UserSettings_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]) ON DELETE CASCADE
              );
              """
            : """
              CREATE TABLE IF NOT EXISTS "UserSettings" (
                  "UserId" uuid NOT NULL,
                  "NotificationSound" boolean NOT NULL DEFAULT TRUE,
                  "CallSound" boolean NOT NULL DEFAULT TRUE,
                  "DesktopNotifications" boolean NOT NULL DEFAULT TRUE,
                  "ReadReceipts" boolean NOT NULL DEFAULT TRUE,
                  "Theme" character varying(20) NOT NULL DEFAULT 'system',
                  "UpdatedAt" timestamp with time zone,
                  CONSTRAINT "PK_UserSettings" PRIMARY KEY ("UserId"),
                  CONSTRAINT "FK_UserSettings_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
              );
              """;
        try
        {
            await _db.Database.ExecuteSqlRawAsync(sql, ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not ensure the UserSettings table; it may already exist or a referenced table is not yet created.");
        }
    }
}

