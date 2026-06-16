namespace Energy.Shared.Models.V1.Operations.WorkOrderAssignment.Responses;

/// <summary>WorkOrderAssignment liste satırı.</summary>
public class WorkOrderAssignmentListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>WorkOrders referansı</summary>
    public Guid WorkOrderId { get; set; }

    /// <summary>Employees referansı</summary>
    public Guid? EmployeeId { get; set; }

    /// <summary>Users referansı</summary>
    public Guid? UserId { get; set; }

    /// <summary>AssignmentRole</summary>
    public string? AssignmentRole { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
