namespace Energy.Shared.Models.V1.Organization.EmployeePosition.Requests;

/// <summary>EmployeePosition güncelleme isteği.</summary>
public class UpdateEmployeePositionRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
