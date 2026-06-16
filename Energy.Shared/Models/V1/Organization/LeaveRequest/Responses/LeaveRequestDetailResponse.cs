namespace Energy.Shared.Models.V1.Organization.LeaveRequest.Responses;

/// <summary>LeaveRequest detay görünümü.</summary>
public class LeaveRequestDetailResponse
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

    /// <summary>LeaveType</summary>
    public string LeaveType { get; set; } = string.Empty;

    /// <summary>StartDate</summary>
    public DateTime StartDate { get; set; }

    /// <summary>EndDate</summary>
    public DateTime EndDate { get; set; }

    /// <summary>Days</summary>
    public decimal Days { get; set; }

    /// <summary>Reason</summary>
    public string? Reason { get; set; }

    /// <summary>Status</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>ApprovalRequestId</summary>
    public Guid? ApprovalRequestId { get; set; }
}
