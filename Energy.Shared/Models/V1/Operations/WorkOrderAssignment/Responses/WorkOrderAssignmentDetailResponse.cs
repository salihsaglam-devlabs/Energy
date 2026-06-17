namespace Energy.Shared.Models.V1.Operations.WorkOrderAssignment.Responses;

/// <summary>WorkOrderAssignment detay görünümü.</summary>
public class WorkOrderAssignmentDetailResponse
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

    /// <summary>WorkOrders referansı</summary>
    public Guid WorkOrderId { get; set; }

    /// <summary>Employees referansı</summary>
    public Guid? EmployeeId { get; set; }

    /// <summary>Users referansı</summary>
    public Guid? UserId { get; set; }

    /// <summary>AssignmentRole</summary>
    public string? AssignmentRole { get; set; }
}
