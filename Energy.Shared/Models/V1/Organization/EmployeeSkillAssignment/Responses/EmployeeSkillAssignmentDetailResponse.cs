namespace Energy.Shared.Models.V1.Organization.EmployeeSkillAssignment.Responses;

/// <summary>EmployeeSkillAssignment detay görünümü.</summary>
public class EmployeeSkillAssignmentDetailResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Oluşturma zamanı</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Oluşturan kullanıcı</summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>Son güncelleme zamanı</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Güncelleyen kullanıcı</summary>
    public Guid? UpdatedBy { get; set; }

    /// <summary>Soft delete bayrağı</summary>
    public bool IsDeleted { get; set; }

    /// <summary>Silinme zamanı</summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>Silen kullanıcı</summary>
    public Guid? DeletedBy { get; set; }

    /// <summary>EmployeeId</summary>
    public Guid EmployeeId { get; set; }

    /// <summary>EmployeeSkillId</summary>
    public Guid EmployeeSkillId { get; set; }

    /// <summary>Level</summary>
    public int Level { get; set; }

    /// <summary>Note</summary>
    public string? Note { get; set; }
}
