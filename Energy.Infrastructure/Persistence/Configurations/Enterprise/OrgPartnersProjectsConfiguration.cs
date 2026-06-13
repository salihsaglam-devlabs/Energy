using Energy.Domain.BusinessPartners;
using Energy.Domain.Core;
using Energy.Domain.Identity;
using Energy.Domain.Organization;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Enterprise;

/// <summary>Organization, BusinessPartners ve Projects modülleri EF Core yapılandırması.</summary>
public static class OrgPartnersProjectsConfiguration
{
    public static void Configure(ModelBuilder b)
    {
        // ---- Organization ----
        b.Entity<EmployeePosition>(e => { e.ToTable("EmployeePositions"); e.HasIndex(x => x.Code).IsUnique(); });
        b.Entity<EmployeeSkill>(e => { e.ToTable("EmployeeSkills"); e.HasIndex(x => x.Code).IsUnique(); });

        b.Entity<Employee>(e =>
        {
            e.ToTable("Employees");
            e.HasIndex(x => x.Code).IsUnique();
            e.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Branch>().WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Department>().WithMany().HasForeignKey(x => x.DepartmentId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<EmployeePosition>().WithMany().HasForeignKey(x => x.EmployeePositionId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<EmployeeSkillAssignment>(e =>
        {
            e.ToTable("EmployeeSkillAssignments");
            e.HasIndex(x => new { x.EmployeeId, x.EmployeeSkillId }).IsUnique();
            e.HasOne<Employee>().WithMany().HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<EmployeeSkill>().WithMany().HasForeignKey(x => x.EmployeeSkillId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<LeaveRequest>(e =>
        {
            e.ToTable("LeaveRequests");
            e.HasOne<Employee>().WithMany().HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<ExpenseClaim>(e =>
        {
            e.ToTable("ExpenseClaims");
            e.HasIndex(x => x.ClaimNo).IsUnique();
            e.HasOne<Employee>().WithMany().HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<ExpenseClaimLine>(e =>
        {
            e.ToTable("ExpenseClaimLines");
            e.HasOne<ExpenseClaim>().WithMany().HasForeignKey(x => x.ExpenseClaimId).OnDelete(DeleteBehavior.Cascade);
        });

        // ---- BusinessPartners ----
        b.Entity<BusinessPartner>(e => { e.ToTable("BusinessPartners"); e.HasIndex(x => x.Code).IsUnique(); });

        b.Entity<BusinessPartnerContact>(e =>
        {
            e.ToTable("BusinessPartnerContacts");
            e.HasOne<BusinessPartner>().WithMany().HasForeignKey(x => x.BusinessPartnerId).OnDelete(DeleteBehavior.Cascade);
        });

        b.Entity<BusinessPartnerAddress>(e =>
        {
            e.ToTable("BusinessPartnerAddresses");
            e.HasOne<BusinessPartner>().WithMany().HasForeignKey(x => x.BusinessPartnerId).OnDelete(DeleteBehavior.Cascade);
        });

        b.Entity<BusinessPartnerBankAccount>(e =>
        {
            e.ToTable("BusinessPartnerBankAccounts");
            e.HasOne<BusinessPartner>().WithMany().HasForeignKey(x => x.BusinessPartnerId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
        });

        // ---- Projects ----
        b.Entity<ProjectType>(e => { e.ToTable("ProjectTypes"); e.HasIndex(x => x.Code).IsUnique(); });
        b.Entity<ProjectStatus>(e => { e.ToTable("ProjectStatuses"); e.HasIndex(x => x.Code).IsUnique(); });

        b.Entity<Project>(e =>
        {
            e.ToTable("Projects");
            e.HasIndex(x => x.Code).IsUnique();
            e.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Branch>().WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<ProjectType>().WithMany().HasForeignKey(x => x.ProjectTypeId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<ProjectStatus>().WithMany().HasForeignKey(x => x.StatusId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<BusinessPartner>().WithMany().HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<User>().WithMany().HasForeignKey(x => x.ManagerUserId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<ProjectLocation>(e =>
        {
            e.ToTable("ProjectLocations");
            e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<ProjectLocation>().WithMany().HasForeignKey(x => x.ParentLocationId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<ProjectPhase>(e =>
        {
            e.ToTable("ProjectPhases");
            e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<ProjectPhase>().WithMany().HasForeignKey(x => x.ParentPhaseId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<ProjectMember>(e =>
        {
            e.ToTable("ProjectMembers");
            e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Employee>().WithMany().HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<ProjectNote>(e =>
        {
            e.ToTable("ProjectNotes");
            e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);
        });
    }
}

