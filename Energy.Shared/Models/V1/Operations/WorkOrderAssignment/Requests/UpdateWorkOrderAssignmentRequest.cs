namespace Energy.Shared.Models.V1.Operations.WorkOrderAssignment.Requests;

/// <summary>WorkOrderAssignment güncelleme isteği.</summary>
public class UpdateWorkOrderAssignmentRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>WorkOrders referansı</summary>
    public Guid WorkOrderId { get; set; }

    /// <summary>Employees referansı</summary>
    public Guid? EmployeeId { get; set; }

    /// <summary>Users referansı</summary>
    public Guid? UserId { get; set; }

    /// <summary>AssignmentRole</summary>
    public string? AssignmentRole { get; set; }
}
