using Energy.Domain.Documents;
using Energy.Domain.Identity;
using Energy.Domain.Notifications;
using Energy.Domain.Reporting;
using Energy.Domain.Workflow;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Enterprise;

/// <summary>Documents, Workflow, Notifications ve Reporting modülleri EF Core yapılandırması.</summary>
public static class DocumentsWorkflowNotificationsReportingConfiguration
{
    public static void Configure(ModelBuilder b)
    {
        // ---- Documents ----
        b.Entity<DocumentFolder>(e =>
        {
            e.ToTable("DocumentFolders");
            e.HasOne<DocumentFolder>().WithMany().HasForeignKey(x => x.ParentFolderId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<Document>(e =>
        {
            e.ToTable("Documents");
            e.HasOne<DocumentFolder>().WithMany().HasForeignKey(x => x.DocumentFolderId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<DocumentVersion>(e =>
        {
            e.ToTable("DocumentVersions");
            e.HasOne<Document>().WithMany().HasForeignKey(x => x.DocumentId).OnDelete(DeleteBehavior.Cascade);
        });

        b.Entity<DocumentRelation>(e =>
        {
            e.ToTable("DocumentRelations");
            e.HasIndex(x => new { x.RelatedModule, x.RelatedEntityType, x.RelatedEntityId });
            e.HasOne<Document>().WithMany().HasForeignKey(x => x.DocumentId).OnDelete(DeleteBehavior.Cascade);
        });

        b.Entity<DocumentPermission>(e =>
        {
            e.ToTable("DocumentPermissions");
            e.HasOne<Document>().WithMany().HasForeignKey(x => x.DocumentId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Role>().WithMany().HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Restrict);
        });

        // ---- Workflow ----
        b.Entity<ApprovalDefinition>(e => { e.ToTable("ApprovalDefinitions"); e.HasIndex(x => x.Code).IsUnique(); });

        b.Entity<ApprovalDefinitionVersion>(e =>
        {
            e.ToTable("ApprovalDefinitionVersions");
            e.HasIndex(x => new { x.ApprovalDefinitionId, x.VersionNo }).IsUnique();
            e.HasOne<ApprovalDefinition>().WithMany().HasForeignKey(x => x.ApprovalDefinitionId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<ApprovalStepDefinition>(e =>
        {
            e.ToTable("ApprovalStepDefinitions");
            e.HasOne<ApprovalDefinitionVersion>().WithMany().HasForeignKey(x => x.ApprovalDefinitionVersionId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<ApprovalStepApprover>(e =>
        {
            e.ToTable("ApprovalStepApprovers");
            e.HasOne<ApprovalStepDefinition>().WithMany().HasForeignKey(x => x.ApprovalStepDefinitionId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<User>().WithMany().HasForeignKey(x => x.ApproverUserId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Role>().WithMany().HasForeignKey(x => x.ApproverRoleId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Energy.Domain.Core.Department>().WithMany().HasForeignKey(x => x.ApproverDepartmentId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<ApprovalCondition>(e =>
        {
            e.ToTable("ApprovalConditions");
            e.HasOne<ApprovalDefinitionVersion>().WithMany().HasForeignKey(x => x.ApprovalDefinitionVersionId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<ApprovalRequest>(e =>
        {
            e.ToTable("ApprovalRequests");
            e.HasIndex(x => new { x.RelatedModule, x.RelatedEntityType, x.RelatedEntityId });
            e.HasOne<ApprovalDefinitionVersion>().WithMany().HasForeignKey(x => x.ApprovalDefinitionVersionId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<User>().WithMany().HasForeignKey(x => x.RequestedByUserId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<ApprovalRequestStep>(e =>
        {
            e.ToTable("ApprovalRequestSteps");
            e.HasOne<ApprovalRequest>().WithMany().HasForeignKey(x => x.ApprovalRequestId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<ApprovalStepDefinition>().WithMany().HasForeignKey(x => x.ApprovalStepDefinitionId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<ApprovalRequestApprover>(e =>
        {
            e.ToTable("ApprovalRequestApprovers");
            e.HasOne<ApprovalRequestStep>().WithMany().HasForeignKey(x => x.ApprovalRequestStepId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<ApprovalAction>(e =>
        {
            e.ToTable("ApprovalActions");
            e.HasOne<ApprovalRequest>().WithMany().HasForeignKey(x => x.ApprovalRequestId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<ApprovalRequestStep>().WithMany().HasForeignKey(x => x.ApprovalRequestStepId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<ApprovalDelegation>(e =>
        {
            e.ToTable("ApprovalDelegations");
            e.HasOne<User>().WithMany().HasForeignKey(x => x.DelegatorUserId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<User>().WithMany().HasForeignKey(x => x.DelegateUserId).OnDelete(DeleteBehavior.Restrict);
        });

        // ---- Notifications ----
        b.Entity<Notification>(e =>
        {
            e.ToTable("Notifications");
            e.HasIndex(x => new { x.RelatedModule, x.RelatedEntityType, x.RelatedEntityId });
        });

        b.Entity<NotificationRecipient>(e =>
        {
            e.ToTable("NotificationRecipients");
            e.HasIndex(x => new { x.UserId, x.IsRead });
            e.HasOne<Notification>().WithMany().HasForeignKey(x => x.NotificationId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<NotificationPreference>(e =>
        {
            e.ToTable("NotificationPreferences");
            e.HasIndex(x => new { x.UserId, x.NotificationType }).IsUnique();
            e.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        });

        // ---- Reporting ----
        b.Entity<ReportDefinition>(e =>
        {
            e.ToTable("ReportDefinitions");
            e.HasIndex(x => x.Code).IsUnique();
            e.HasOne<Permission>().WithMany().HasForeignKey(x => x.RequiredPermissionCode)
                .HasPrincipalKey(p => p.Code).OnDelete(DeleteBehavior.SetNull);
        });

        b.Entity<DashboardWidget>(e =>
        {
            e.ToTable("DashboardWidgets");
            e.HasIndex(x => x.Code).IsUnique();
            e.HasOne<Permission>().WithMany().HasForeignKey(x => x.RequiredPermissionCode)
                .HasPrincipalKey(p => p.Code).OnDelete(DeleteBehavior.SetNull);
        });
    }
}

